using TMPro;
using UnityEngine;

public class WalletVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        Wallet.OnMoneyChanged += ShowUi;
    }

    public void ShowUi(int amount)
    {
        _text.text = amount.ToString();
    }
}