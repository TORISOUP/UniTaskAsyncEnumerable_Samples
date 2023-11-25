using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TORISOUP.UniTaskAsyncEnumerableSamples.UIEventDiff
{
    public class UIEventDiffSample : MonoBehaviour
    {
        [SerializeField] private Button _buttonA;
        [SerializeField] private Text _outputText;

        private void Start()
        {
            SubscribeAsObservable(destroyCancellationToken);
            SubscribeAsAsyncEnumerable(destroyCancellationToken);
        }

        // ButtonをObservableとして購読
        private void SubscribeAsObservable(CancellationToken ct)
        {
            _buttonA.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _outputText.text += "ButtonA Clicked (Observable)!\n";
                })
                .AddTo(ct);
        }

        // ButtonをUniTaskAsyncEnumerableとして購読
        private void SubscribeAsAsyncEnumerable(CancellationToken ct)
        {
            _buttonA.OnClickAsAsyncEnumerable(ct)
                .Subscribe(_ =>
                {
                    _outputText.text += "ButtonA Clicked (UniTaskAsyncEnumerable)!\n";
                }, ct);
        }
    }
}