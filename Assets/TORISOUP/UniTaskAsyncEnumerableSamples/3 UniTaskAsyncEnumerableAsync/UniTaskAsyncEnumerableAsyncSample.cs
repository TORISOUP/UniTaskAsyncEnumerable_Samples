using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TORISOUP.UniTaskAsyncEnumerableSamples.UniTaskAsyncEnumerableAsync
{
    public class UniTaskAsyncEnumerableAsyncSample : MonoBehaviour
    {
        [SerializeField] private Button _buttonA;
        [SerializeField] private InputField _urlInputField;
        [SerializeField] private Text _outputText;

        private void Start()
        {
            SubscribeAsUniTaskAsyncEnumerable(destroyCancellationToken);
        }

        // ButtonをObservableとして購読
        private void SubscribeAsUniTaskAsyncEnumerable(CancellationToken ct)
        {
            // ボタンが押されたら通信を実行
            // 通信結果をTextに表示する
            _buttonA.OnClickAsAsyncEnumerable(ct)
                // ForEachAwaitAsyncになっている点に注意
                .ForEachAwaitAsync(async _ =>
                {
                    Debug.Log("<color=red>実行開始！</color>");
                    _outputText.text = "";

                    var url = _urlInputField.text;

                    // サーバに問い合わせる
                    var result = await FetchAsync(url, ct);
                    _outputText.text = result;

                    // ボタンを連打したときに一気に通信しないように3秒待たせる
                    await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: ct);

                    Debug.Log("<color=green>実行終了！</color>");
                }, ct);
        }


        private async UniTask<string> FetchAsync(string url, CancellationToken ct)
        {
            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest().ToUniTask(cancellationToken: ct);
            return request.downloadHandler.text;
        }
    }
}