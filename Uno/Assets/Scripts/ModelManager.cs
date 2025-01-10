using UnityEngine;
using Unity.Sentis;
using System.Collections.Generic;
using System.Linq;

public class ModelManager : MonoBehaviour
{
    public ModelAsset modelAsset;
    public ModelAsset modelColour;

    private Model runtimeModel;
    private Model colourModel;

    public Worker worker;
    private Worker colourWorker;

    private int chosencard;
    private List<float> scores = new List<float>();

    void Start()
    {
        runtimeModel = ModelLoader.Load(modelAsset);
        worker = new Worker(runtimeModel, BackendType.GPUCompute);

        colourModel = ModelLoader.Load(modelColour);
        colourWorker = new Worker(colourModel, BackendType.GPUCompute);

        // Log input shapes for debugging
        List<Model.Input> inputs = runtimeModel.inputs;
        foreach (var input in inputs)
        {
            Debug.Log("Input Name: " + input.name);
            Debug.Log("Input Shape: " + string.Join(", ", input.shape));
        }

        List<Model.Input> colourInputs = colourModel.inputs;
        foreach (var input in colourInputs)
        {
            Debug.Log("Colour Model Input Name: " + input.name);
            Debug.Log("Colour Model Input Shape: " + string.Join(", ", input.shape));
        }
    }

    void OnDestroy()
    {
        worker?.Dispose();  // Dispose the worker when done
        colourWorker?.Dispose();  // Dispose the color worker when done
    }

    public int runmodel1(List<(int, int)> aiHand, (int, int) lastcardplayed)
    {
        List<List<(int, int)>> nextStates = NextPossibleStates(aiHand, lastcardplayed);

        var flattenedStates = nextStates
            .Select(state => FlattenTuples(state.Select(card => ((float)card.Item1, (float)card.Item2)).ToList()))
            .ToArray();

        int bestStateIndex = -1;
        float bestScore = float.MinValue;

        for (int i = 0; i < flattenedStates.Length; i++)
        {
            float score = RunModelCard(flattenedStates[i]);

            if (score > bestScore)
            {
                bestScore = score;
                bestStateIndex = i;
            }
        }

        chosencard = (bestStateIndex != -1) ? bestStateIndex : -1;
        return chosencard;
    }

    public (int,int) runmodel2(List<(int, int)> aiHand)
    {
        List<int> aiHandCol = GetAiHandCol(aiHand);
        float[] flattenedColors = aiHandCol.Select(col => (float)col).ToArray();

        scores.Clear();
        chosencard = RunColourModel(flattenedColors, aiHand, 0);
        Debug.Log($"chosen card is {chosencard}");
        (int,int) chosencardReturn = (14, chosencard);

        return chosencardReturn;
    }

    private float RunModelCard(float[] flattenedCardData)
    {
        TensorShape shape = new TensorShape(1, flattenedCardData.Length);
        Tensor<float> tensor = new Tensor<float>(shape, flattenedCardData);
        worker.Schedule(tensor);

        Tensor<float> outputTensor = worker.PeekOutput() as Tensor<float>;
        float score = float.MinValue;

        if (outputTensor != null)
        {
            var cpuTensor = outputTensor.ReadbackAndClone();
            if (cpuTensor.count > 0)
            {
                score = cpuTensor[0]; // Assuming the model outputs a single score
            }
            outputTensor.Dispose(); // Dispose the output tensor
            cpuTensor.Dispose();    // Dispose the cloned CPU tensor
        }
        tensor.Dispose(); // Dispose the input tensor

        return score;
    }

    private int RunColourModel(float[] flattenedColors, List<(int, int)> originalState, int stateIndex)
    {
        TensorShape shape = new TensorShape(1, flattenedColors.Length);
        Tensor<float> tensor = new Tensor<float>(shape, flattenedColors);
        colourWorker.Schedule(tensor);

        Tensor<float> outputTensor = colourWorker.PeekOutput() as Tensor<float>;
        int bestIndex = -1;

        if (outputTensor != null)
        {
            var cpuTensor = outputTensor.ReadbackAndClone();
            List<float> scores = new List<float>();

            for (int i = 0; i < cpuTensor.count; i++)
            {
                float score = cpuTensor[i];
                scores.Add(score);
            }

            float maxScore = scores.Max();
            bestIndex = scores.IndexOf(maxScore) + 1;

            outputTensor.Dispose(); // Dispose the output tensor
            cpuTensor.Dispose();    // Dispose the cloned CPU tensor
        }
        tensor.Dispose(); // Dispose the input tensor

        return bestIndex;
    }

    public static float[] FlattenTuples(List<(float, float)> tupleList)
    {
        return tupleList.SelectMany(tuple => new[] { tuple.Item1, tuple.Item2 }).ToArray();
    }

    public List<List<(int, int)>> NextPossibleStates(List<(int, int)> aiHand, (int, int) lastcardplayed)
    {
        List<List<(int, int)>> nextStates = new List<List<(int, int)>>();

        nextStates.Add(new List<(int, int)> { lastcardplayed, (-2, -1) });

        foreach (var card in aiHand)
        {
            nextStates.Add(new List<(int, int)> { lastcardplayed, card });
        }

        return nextStates;
    }

    private List<int> GetAiHandCol(List<(int, int)> aiHand)
    {
        List<int> aiHandCol = aiHand.Select(card => card.Item2).ToList();
        while (aiHandCol.Count < 12)
        {
            aiHandCol.Add(-1);
        }
        return aiHandCol.Take(12).ToList();
    }
}