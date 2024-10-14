using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������Ʈ�� ���� ��ũ��Ʈ. �÷��̾ �ٷ� ���� ��ġ, �ʱ�ȭ ���� Ŭ����
/// </summary>
public class Weapon : MonoBehaviour
{
    public int id;  // ���� ����ũ ���̵�
    public int prefabId;    // PoolManager�� ��ϵ� prefab id
    public float damage;
    public int count;
    public float speed;

    float timer; // ���� ������ �ΰ� ���Ÿ� ���� �߻�
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }


    private void Update()
    {
        if (!GameManager.instance.isLive)   // �ð� �帧
            return;

        switch (id)
        {
            case 0: // ȸ�� ����
                // ȸ�� �Ѵ�
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:    // ���Ÿ� ����
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1); 
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if(id == 0)
        {
            Batch();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);   // ���� ������ ���� ���⿡ �����ϱ� ����(������ ����ȵ� ���λ��� ���� ����)
    }


    /// <summary>
    /// id�� ���� �ʱ갪
    /// </summary>
    public void Init(ItemData data)
    {
        // Basic set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;


        // Property set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        // Ǯ���� ����ִ� �����հ��� data.projectile�� ������ ���ؼ� ������ �������� ã�´�.
        // ��ũ��Ʈ�� ������Ʈ�� �������� ���ؼ� �ε����� �ƴ� ���������� ���Ѵ�
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            // ������ ���̵�� Ǯ�� �Ŵ����� �������� ã�Ƽ� �ʱ�ȭ 
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0: // ȸ�� ����
                speed = 150 * Character.WeaponSpeed;    // ȸ�� �ӵ�
                Batch();
                break;
            default:
                // ���Ÿ� ����
                speed = 0.4f * Character.WeaponRate;   // ���� �ӵ�
                break;
        }

        // Hand set
        Hand hand = player.hands[(int)data.itemType];   // �ٰŸ�, ���Ÿ�
        hand.spriter.sprite = data.hand;    // ��ũ��Ʈ�� ������Ʈ�� �����ͷ� ��������Ʈ ����
        hand.gameObject.SetActive(true);

        // BroadcastMessage : Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // ���� ������ ���� ���⿡ �����ϱ� ����(������ ����ȵ� ���λ��� ���� ����)
    }




    /// <summary>
    /// ���� ������ ��ġ ��ġ
    /// </summary>
    void Batch()
    {
        for (int index =0; index < count; index++)
        {
            
            Transform bullet;
            
            // ���� ������Ʈ�� ���� Ȱ���ϰ� ���ڶ� ���� Ǯ������ �������� 
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;  // �θ� ������Ʈ�� ��ġ�� ������ Weapon ���Ϸ� ��.
            }
            

            // ���� ���� ��ġ �ʱ�ȭ 
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count; // 360���� ������ŭ ������ ������ ���� �Ѵ�.
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World); // local �÷��̾��� y���̷� 1.5f�Ÿ��� �ΰ� �̵������� world����
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity per.
        }
    }

    
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;   // �Ÿ� : ũ��� ����
        dir = dir.normalized; // normalized : ���� ������ ������ �����ϰ� ũ�⸦ 1�� ��ȯ�ϴ� �Ӽ�

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // count : �����

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);  // �߻� ȿ����

    }

}
