using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Joystick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rect_background;
    [SerializeField] private RectTransform rect_Joystick;

    private float radius;

    [SerializeField] private GameObject go_player;
    [SerializeField] private float movespeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 movePosition;

    private bool isTouch = false;
  

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        radius = rect_background.rect.width * 0.5f; //������
       animator = go_player.GetComponent<Animator>(); //go_player ���� ��ü���� �ִϸ����� ���� ��Ҹ� �����´�.
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("iswalk", isTouch); //�ִϸ��̼� iswlak �� tru,false
        if (isTouch) {   //is walk�� true�� �÷��̾��� �������� �̵��� ȸ��
            go_player.transform.position += movePosition;

            RotatePlayer();
        }
    }
    void RotatePlayer() {
        Vector3 inputDirection = movePosition.normalized; //moveposition ����ȭ�Ѱ��� inputDirection�� �־��ش�
        if (inputDirection.magnitude > 0.1f) {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up); //�Ӹ��� ������ ���ϵ����Ͽ� ȸ��
            go_player.transform.rotation = Quaternion.Lerp(go_player.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            // rotationSpeed�� ��ŭ�� �ӵ��� ȸ��
        }
    }
    public void OnDrag(PointerEventData eventData) { //�巡�׸� ������ �̺�Ʈ �߻�

        Vector2 value= eventData.position - (Vector2)rect_background.position;  //�̺�Ʈ �����ǿ� �������ǰ��� ������ value�� �־��ش�
     
        value=Vector2.ClampMagnitude(value, radius); //value���� �ƹ��� Ŀ���� �ִ� radius���� ��������
       rect_Joystick.localPosition = value; //���̽�ƽ�� ������ǥ�� value���� �ǵ���
        //* position�� �⺻������ ������ǥ�̴� localposition�� ���� ������ �θ�������� ������ �ϱ⶧��

        float distance= Vector2.Distance(rect_background.position,rect_Joystick.position) / radius;//distance�� ��׶��� �����ǿ� ���̽�ƽ �������� ������ ���������� �������� �־��ش�
        //*Distance(A,B): A-B
        value = value.normalized;//value�� ����ȭ�� ���� value���� �־��ش�
        movePosition = new Vector3(value.x * movespeed *distance* Time.deltaTime,0f,value.y*movespeed*distance*Time.deltaTime);
        //movePosition�� x���� value.x * movespeed *distance* Time.deltaTime y�࿡ 0 z�࿡ value.y*movespeed*distance*Time.deltaTime�� �ִ´�
        //value.x�� ���̽�ƽ�� x��ǥ�� value.y�� ���̽�ƽ�� y��ǥ��
    }

    public void OnPointerDown(PointerEventData eventData) { //�������� �̺�Ʈ �߻�
        isTouch = true; //isTouch �� true 
    }

    public void OnPointerUp(PointerEventData eventData) {//������ �̺�Ʈ �߻�
        isTouch = false;//isTouch �� false 
        rect_Joystick.localPosition = Vector3.zero;//���̽�ƽ�� ������ǥ�� 0�� �ǵ���
        movePosition =Vector3.zero; //�̵���ǥ���� 0�� �ǵ���
    }
}
