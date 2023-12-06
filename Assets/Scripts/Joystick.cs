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
        radius = rect_background.rect.width * 0.5f; //반지름
       animator = go_player.GetComponent<Animator>(); //go_player 게임 개체에서 애니메이터 구성 요소를 가져온다.
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("iswalk", isTouch); //애니매이션 iswlak 가 tru,false
        if (isTouch) {   //is walk가 true면 플레이어의 포지션이 이동과 회전
            go_player.transform.position += movePosition;

            RotatePlayer();
        }
    }
    void RotatePlayer() {
        Vector3 inputDirection = movePosition.normalized; //moveposition 정규화한값을 inputDirection에 넣어준다
        if (inputDirection.magnitude > 0.1f) {
            Quaternion toRotation = Quaternion.LookRotation(inputDirection, Vector3.up); //머리가 위쪽을 향하도록하여 회전
            go_player.transform.rotation = Quaternion.Lerp(go_player.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            // rotationSpeed값 만큼의 속도로 회전
        }
    }
    public void OnDrag(PointerEventData eventData) { //드래그를 했을때 이벤트 발생

        Vector2 value= eventData.position - (Vector2)rect_background.position;  //이벤트 포지션에 백포지션값을 뺀값을 value에 넣어준다
     
        value=Vector2.ClampMagnitude(value, radius); //value값이 아무리 커져도 최대 radius값을 가지도록
       rect_Joystick.localPosition = value; //조이스틱의 로컬좌표가 value값이 되도록
        //* position은 기본적으로 월드좌표이다 localposition을 쓰는 이유는 부모기준으로 잡으려 하기때문

        float distance= Vector2.Distance(rect_background.position,rect_Joystick.position) / radius;//distance에 백그라운드 포지션에 조이스틱 포지션을 뺀값을 반지름으로 나눈값을 넣어준다
        //*Distance(A,B): A-B
        value = value.normalized;//value의 정규화한 값을 value값에 넣어준다
        movePosition = new Vector3(value.x * movespeed *distance* Time.deltaTime,0f,value.y*movespeed*distance*Time.deltaTime);
        //movePosition의 x값에 value.x * movespeed *distance* Time.deltaTime y축에 0 z축에 value.y*movespeed*distance*Time.deltaTime을 넣는다
        //value.x는 조이스틱의 x좌표값 value.y는 조이스틱의 y좌표값
    }

    public void OnPointerDown(PointerEventData eventData) { //눌렀을때 이벤트 발생
        isTouch = true; //isTouch 가 true 
    }

    public void OnPointerUp(PointerEventData eventData) {//땠을때 이벤트 발생
        isTouch = false;//isTouch 가 false 
        rect_Joystick.localPosition = Vector3.zero;//조이스틱의 로컬좌표가 0이 되도록
        movePosition =Vector3.zero; //이동좌표값이 0이 되도록
    }
}
