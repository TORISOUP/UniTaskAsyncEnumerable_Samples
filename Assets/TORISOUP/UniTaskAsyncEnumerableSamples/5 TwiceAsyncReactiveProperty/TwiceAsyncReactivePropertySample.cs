using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TORISOUP.UniTaskAsyncEnumerableSamples.AsyncReactivePropertySample
{
    public class TwiceAsyncReactivePropertySample : MonoBehaviour
    {
        [SerializeField] private Button _buttonA;
        [SerializeField] private Text _outputTextA;
        [SerializeField] private Text _outputTextB;

        // カウントアップを続けるUniTaskAsyncEnumerable
        private IUniTaskAsyncEnumerable<int> CreateAsyncEnumerable()
        {
            // AsyncReactivePropertyを使って生成する
            var asyncReactiveProperty = new AsyncReactiveProperty<int>(0);

            UniTask.Void(async () =>
            {
                while (!destroyCancellationToken.IsCancellationRequested)
                {
                    // ボタンが押されたらカウントアップ
                    await _buttonA.OnClickAsync(destroyCancellationToken);
                    asyncReactiveProperty.Value++;
                    
                    Debug.Log($"{Time.time}: {asyncReactiveProperty.Value}");
                }
            });

            Debug.Log("AsyncReactivePropertyを生成しました");
            return asyncReactiveProperty;
        }

        private void Start()
        {
            // カウントアップを続けるUniTaskAsyncEnumerableを作成
            // 実体はAsyncReactiveProperty
            var uniTaskAsyncEnumerable = CreateAsyncEnumerable();

            // 購読してText Aに出力する
            uniTaskAsyncEnumerable
                .ForEachAsync(i =>
                {
                    _outputTextA.text += $"A: {i}\n";
                }, destroyCancellationToken);

            
            // 購読してText Bに出力する
            uniTaskAsyncEnumerable
                .ForEachAsync(i =>
                {
                    _outputTextB.text += $"B: {i}\n";
                }, destroyCancellationToken);
        }
    }
}