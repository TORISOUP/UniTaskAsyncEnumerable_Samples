using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TORISOUP.UniTaskAsyncEnumerableSamples.ObservableAsync
{
    public class ObservableAsyncSample : MonoBehaviour
    {
        [SerializeField] private Button _buttonA;
        [SerializeField] private InputField _urlInputField;
        [SerializeField] private Text _outputText;

        private void Start()
        {
            SubscribeAsObservable(destroyCancellationToken);
        }

        // ButtonをObservableとして購読
        private void SubscribeAsObservable(CancellationToken ct)
        {
            // ボタンが押されたら通信を実行
            // 通信結果をTextに表示する
            _buttonA.OnClickAsObservable()
                .Subscribe(async _ =>
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
                })
                .AddTo(ct);
        }
        
        private async UniTask<string> FetchAsync(string url, CancellationToken ct)
        {
            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest().ToUniTask(cancellationToken: ct);

            return request.downloadHandler.text;
        }
    }
}