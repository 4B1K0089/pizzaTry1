using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // �[�J�o��

public class CharacterSlot : MonoBehaviour
{
    public int characterIndex;
    public Image characterImage; // ��ܨ���Ϲ�
    public List<Image> playerColorIndicators = new List<Image>(); // ��ܪ��a�C�⪺�ϰ�

    public bool isSelected = false;

    public void LockSlot()
    {
        isSelected = true;
        characterImage.color = Color.gray; // ��ܫ�Ǧ����
    }

    public void AddPlayerColor(int playerIndex)
    {
        // �ھڪ��a���޵��C��
        Color playerColor = GetPlayerColor(playerIndex);
        if (!playerColorIndicators[playerIndex].enabled)
        {
            playerColorIndicators[playerIndex].color = playerColor;
            playerColorIndicators[playerIndex].enabled = true;
        }
    }

    private Color GetPlayerColor(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0: return Color.green; // ���a 1: ���
            case 1: return Color.blue;  // ���a 2: �Ŧ�
            case 2: return Color.yellow; // ���a 3: ����
            case 3: return Color.red;   // ���a 4: ����
            default: return Color.white;
        }
    }
}
