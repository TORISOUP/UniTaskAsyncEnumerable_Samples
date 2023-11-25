using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TORISOUP.UniTaskAsyncEnumerableSamples.UniTaskAsyncEnumerableInstance
{
    public class UniTaskAsyncEnumerableAsyncSample : MonoBehaviour
    {
        [SerializeField] private Button _buttonA;
        [SerializeField] private Button _buttonB;
        [SerializeField] private Text _outputText;

        // カウントアップを続けるUniTaskAsyncEnumerable
        private IUniTaskAsyncEnumerable<int> CountUpAsyncEnumerable()
        {
            return UniTaskAsyncEnumerable.Create<int>(async (writer, _) =>
            {
                for (var i = 0; i < 10; i++)
                {
                    Debug.Log($"{Time.time}: {i}");
                    await writer.YieldAsync(i);
                }
            });
        }

        private void Start()
        {
            // カウントアップを続けるUniTaskAsyncEnumerableをインスタンス化した"つもり"
            var uniTaskAsyncEnumerable = CountUpAsyncEnumerable();

            // ボタンAが押されたら1個取得する
            uniTaskAsyncEnumerable
                .ForEachAwaitAsync(async i =>
                {
                    _outputText.text += $"A: {i}\n";
                    await _buttonA.OnClickAsync(destroyCancellationToken);
                }, destroyCancellationToken);
            
            // ボタンBが押されたら1個取得する
            uniTaskAsyncEnumerable
                .ForEachAwaitAsync(async i =>
                {
                    _outputText.text += $"B: {i}\n";
                    await _buttonB.OnClickAsync(destroyCancellationToken);
                }, destroyCancellationToken);

        }
    }
}