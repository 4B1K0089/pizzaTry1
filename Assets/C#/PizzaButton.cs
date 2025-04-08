using UnityEngine;
using UnityEngine.UI;

public class PizzaButton : MonoBehaviour
{
    public Image pizzaImage; // ���s�W��ܪ����ĹϹ�
    public Sprite[] pizzaSprites; // �i�諸���ĹϹ�

    // �ΨӪ�l�Ʃ��ĹϤ�
    public void SetPizzaImage(int pizzaIndex)
    {
        pizzaImage.sprite = pizzaSprites[pizzaIndex]; // ��s���s�W�����ĹϤ�
    }

    // ���a��ܩ��ī᪺�^��
    public void OnButtonClick()
    {
        // �I�s CharacterSelectionManager ���� OnPizzaButtonClicked ��k
        int playerIndex = int.Parse(gameObject.name.Substring(10)) - 1; // �ھګ��s�W�٨��o���a����
        CharacterSelectionManager.Instance.OnPizzaButtonClicked(playerIndex);
    }
}
