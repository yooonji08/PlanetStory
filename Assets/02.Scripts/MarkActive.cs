using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkActive : MonoBehaviour
{
    public GameObject talkBtn; // ��ȭ�ϱ� ��ư

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ ��ũ ����(! ����)�� ���Դٸ�
        if (other.CompareTag("PLAYER"))
        {
            if (PlanetMng.instance.planetState == PlanetState.Default) // ó�� ���¶��
            {
                talkBtn.gameObject.SetActive(true); // ��ȭ�ϱ� ��ư Ȱ��ȭ
            }
            else if (PlanetMng.instance.planetState == PlanetState.Mission) // �̼� ���¶��
            {
                PlanetMng.instance.noClearMission(); // ���� �����ϴٴ� �ؽ�Ʈ ���
            }
            else if (PlanetMng.instance.planetState == PlanetState.Result) // �̼��� �Ϸ��� ���¶��
            {
                PlanetMng.instance.getClearMission1(); // �ؽ�Ʈ ���� ���
            }
        }
    }
}
