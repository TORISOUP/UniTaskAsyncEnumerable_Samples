using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TORISOUP.UniTaskAsyncEnumerableSamples.ChannelSample
{
    public class ChannelSample : MonoBehaviour
    {
        [SerializeField] private Button _buttonA;
        [SerializeField] private Text _outputTextA;
        [SerializeField] private Text _outputTextB;

        // カウントアップを続けるUniTaskAsyncEnumerable
        private IUniTaskAsyncEnumerable<int> CreateAsyncEnumerable()
        {
            // Channelを使って生成する
            var channel = Channel.CreateSingleConsumerUnbounded<int>();
            var writer = channel.Writer;
            destroyCancellationToken.Register(() => writer.TryComplete());
            
            var value = 0;
            UniTask.Void(async () =>
            {
                writer.TryWrite(value++);

                while (!destroyCancellationToken.IsCancellationRequested)
                {
                    // ボタンが押されたらカウントアップ
                    await _buttonA.OnClickAsync(destroyCancellationToken);
                    writer.TryWrite(value++);
                    Debug.Log($"{Time.time}: {value}");
                }
            });

            Debug.Log("Channelを生成しました");
            return channel.Reader.ReadAllAsync(destroyCancellationToken);
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
            // ここで例外が出る
            uniTaskAsyncEnumerable
                .ForEachAsync(i =>
                {
                    _outputTextB.text += $"B: {i}\n";
                }, destroyCancellationToken);
        }
    }
}
