# DeadRising
메타버스 아카데미 2차 프로젝트(2022.7.26 ~ 2022.8.28)  

## 게임 시연 영상
* Youtube 링크  
  
    [![Video Label](http://img.youtube.com/vi/BcmCwdvn-R0/1.jpg)](https://youtu.be/BcmCwdvn-R0)  
      
##  게임  
  * **DEAD RISING**   
    <img src="Image/logoImage.jpg" height="200px">  

## 팀 구성 및 역할
   <img src="Image/teamRole.png" width="800px">  

## 게임 시퀀스
  <img src="Image/GameSequence.png" height="500px">  

## 사용 에셋
  * **[POLYGON Apocalypse](https://assetstore.unity.com/packages/3d/environments/urban/polygon-apocalypse-low-poly-3d-art-by-synty-154193)**  
    <img src="Image/AssetImage.png" style="width:400px"></img>  

----
# 주요 구현 요소(김혜성)

## 시네마틱 영상

* **시네머신**  

  * 오프닝 씬  
    <img src="Image/openingCinemachine.png" height="200px"></img>  

  * 엔딩 크레딧  
    <img src="Image/endingCinemachine.png" height="200px"></img>  
  
## 플레이어

* **플레이어 모델**  
    
    <img src="Image/CharModel.png" height="200px"></img>  
* **무기 모델** : (왼쪽부터) 일반 석궁, 폭탄 석궁, 기관총  
  
    <img src="Image/arrow.png" height="100px"></img> <img src="Image/bombArrow.png" height="100px"></img> <img src="Image/Gun.png" height="100px"></img>    


## 애니메이션 : 캐릭터 애니메이션 & Rig 애니메이션 
  * **캐릭터 애니메이션**  
    *  **Root Motion 사용**  
      *  이전 프로젝트에서는 애니메이션과 위치 이동을 따로 구현하였는데 이번 프로젝트에서는 Root Motion 사용  
      *  Root Motion이 적용된 애니메이션은 애니메이션과 이동속도가 싱크가 맞게 만들어져 있어서 보다 자연스러운 이동 구현 가능  
          (ex 기존에는 발걸음을 옮기기 전인데도 불구하고 위치 이동, 하지만 RootMotion 적용시 정확히 발걸음을 옮겼을시 이동)   
          <img src="Image/rootMotion.gif" style="width:300px"></img>   
      *  **사전 작업**  
          * 유니티에서 제공하는 애니메이션 콜백함수인 void OnAnimatorMove() 구현  
            ```C#
            private void OnAnimatorMove()
            {
                rootMotion += animator.deltaPosition;
            }
            ```
           *  그리고 (Vector3)rootMotion에 적절한 값을 곱해준후 CharacterController.Move로 해당 값만큼 이동 후 0으로 초기화 시켜준다.(누적 방지)
              ```C#
              private void UpdateOnGround()
              {   
                  Vector3 stepForwardAmount = rootMotion* groundSpeed;
                  //중력
                  Vector3 stepDownAmount = Vector3.down * stepDown;

                  cc.Move(stepForwardAmount + stepDownAmount);
                  rootMotion = Vector3.zero;
              }
              ```  
    *  **Base Layer** : 걷기, 뛰기, 점프  
        *  **Animator**  
          
            <img src="Image/Anim_BaseLayer.png" width="500px"></img>  
        * **Unity Blend Tree** : 걷기, 뛰기 State는 Blend Tree로 구성되어 방향키에 따라 자연스러운 움직임  
         
            <img src="Image/Anim_Blend.png" height="300px"></img>  
    *  **Aim Layer** : 조준시 카메라 줌 인/줌 아웃  
        *  **Animator**  
          
            <img src="Image/Anim_camLayer.png" height="300px"></img>  
            
  * **Rig 애니메이션**  
    * **개요**
      *  장착한 무기에 따라 캐릭터의 팔 애니메이션이 다르고 무기와 함께 팔이 움직여야 하기 때문에 RigLayer 세팅  
      
          <img src="Image/RigSetting.png" height="300px"></img>  
      *  Multi-Parent Constraint와 Two Bone IK Constraint를 통해 각 종 애니메이션을 **최소의 컴포넌트**로 손쉽게 제작  
          * 실제 캐릭터 rig가 아니라 따로 세팅한 컴포넌트로 애니메이션 제작  
             
            <img src="Image/AnimComponent.png" height="300px"></img>  
          * 작업 Gif   
            
            <img src="Image/MakingAnim.gif" style="width:400px"></img>  
    * **구현 Gif**  
      *  **반동 및 재장전**  
          <img src="Image/shotArrow.gif" style="width:300px"></img>  <img src="Image/shotGun.gif" style="width:300px"></img>  
      *  **스왑**  
          <img src="Image/swapWeapon.gif" style="width:300px"></img>  
      *  **무기 장착시 뛰기 모션**  
          <img src="Image/RunArrow.gif" style="width:300px"></img> <img src="Image/RunGun.gif" style="width:300px"></img>   

    * **Animator**  :  일반 석궁과 폭탄 석궁은 공통 모션이기때문에 석궁과 기관총 모션 2세트로 구성
      *  **Base Layer** : 총 스왑 & 재장전 모션    
          *  **Animator**  

              <img src="Image/RigAnim_Base.png" width="500px"></img>  
      *  **Reciol Layer** : 총 반동 모션      
          *  **Animator**  

              <img src="Image/RigAnim_Recoil.png" width="500px"></img>  
      *  **Sprinting Layer** : 총 장착 후 달릴때 모션 
          *  **Animator**  

              <img src="Image/RigAnim_Sprint.png" width="500px"></img>  
            

## 게임 로직 및 기능

* **PlayerController.cs**  
  *  **Main Function**  : Update()에서 사용자의 입력에 따라 캐릭터 이동 제어
      *  **void UpdateOnGround()** : (지면) 캐릭터 이동 제어  
          *  Sub Function  
              *  **bool IsSprinting()** : 총 발사, 장전, 스왑, 조준 상태 체크  
                  ```C#
                  return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
                  ```  
              *  **void UpdateIsSprinting()** : bool IsSprinting()를 받아 애니메이션 적용  
              *  **void Jump()** : float jumpVelocity 값 세팅 및 void SetinAir 호출  
              *  **void SetinAir(float jumpVelocity)** : void UpdateInAir()로 전환, 점프 anim 재생.
      *  **void UpdateInAir()** : (공중) 캐릭터 이동 제어    
          *  Sub Function
              *  **Vector3 CalculateAirControl()** : 방향키 입력에 따라 공중에서 움직임 벡터값 설정  
              
  *  **Animation Event Function** : 발걸음에 따라 발자국 소리 재생  
      *  **void insideStep()** : 랜덤 오디오 클립 재생  
          *  **AudioClip GetRandomClip(int a, int b)** : 랜덤 오디오 클립 반환  
  *  **Unity Function** : 유니티에서 제공하는 함수  
      *  **void OnAnimatorMove()** : Root Motion 관리  
      *  **void OnControllerColliderHit(ControllerColliderHit hit)** : 오브젝트가 CharacterController와 충돌시 밀리게 하기 위해 사용   


* **ActiveWeapon.cs**  
  *  **Main Function**  : Update()에서 사용자의 입력 및 캐릭터 속성(레벨 등)에 따라 무기를 관리하는 함수  
      *  **void HandleFireWeapon()** : 무기 발사와 장전 제어      
      *  **void HandleSwapWeapon()** : 무기 스왑 제어    
      *  **void UpgradeWeaponSystem()** : 레벨에 따른 무기 업그레이드 관리        

  *  **Sub Function**  
      *  **RayCastWeapon1 GetActiveWeapon() / GetWeaPon(int index)** : 무기 객체를 가져오는 함수     
      *  **void Equip(RayCastWeapon1 newWeapon, bool equipNow=true)** : 무기를 얻는 함수, 얻자마자 장착할 여부 선택 가능       
      *  **void SetActiveWeapon(WeaponSlot weaponSlot)** : SwitchWeapon 전달인수 세팅 및 코루틴 실행
          *  **IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)**  
              : rig 애니메이션 파라미터 세팅 및 HolsterWeapon,ActivateWeapon코루틴 순차적 실행, activeWeaponIndex 세팅  
              * **IEnumerator HolsterWeapon(int index)** : 무기 배낭에 보관 기능, 애니메이션 및 변경 중 bool 값 세팅  
              * **IEnumerator ActivateWeapon(int index)** : 무기 장착 기능, 애니메이션 및 변경 중 bool 값 세팅, 무기 활성화 UI 표현       
      *  **void RefillAmmo(int clipCount),(RayCastWeapon1 weapon ,int clipCount)** : 무기 탄창 추가 및 UI 업데이트  
      *  **IEnumerator ToggleActiveWeapon()** : 맨손 상태로 전환하는 함수  
      *  **void EquipFirstWeapon()** : 처음 플레이어가 생성됐을때 기본무기를 장착하는 함수  
      *  **bool IsFiring()** : 현재 무기 발사중인지 체크하는 함수  

* **ReloadWeapon.cs**  
  *  **Animation Event Function**  : 애니메이터와 스크립트가 동일객체에 있어야 애니메이션 이벤트 함수로 등록할수있는데 플레이어는 2개의 애니메이터를 가지고 있다.
      **ReloadWeapon.cs**스크립트는 플레이어에 있어야하지만(관리적인 측면) 여기의 함수는 rig 애니메이터의 이벤트함수로 등록해야한다. 그래서 rig오브젝트에는 **WeaponAnimationEvents.cs**를 붙여준뒤 UnityEvent 기능을 활용하여 AddListener(OnAnimationEvent)로 **ReloadWeapon.cs**에 있는 함수들을 이벤트함수로 등록할 수 있게 해주었다.    <img src="Image/eventFunction.png" width="500px"></img>
      *  **void OnAnimationEvent(string eventName)** : 매개변수에 따라 스위치문을 통해 이벤트 함수 실행.  
          *  **void DetachMagazine()** : 탄창 분리 기능(원래 무기에 붙어있는 탄창을 비활성화 후 임시 탄창을 손 위치에 생성한다), 장전 소리 재생.    
          *  **void DropMagazine()** : 탄창 버리는 기능(손에 있는 탄창을 비활성화 후 떨어지는 탄창을 생성 후 떨어지게 한다(rigidbody 및 collider 추가))        
          *  **void RefillMagazine()** : 배낭에서 탄창을 꺼내는 기능(손에 있는 탄창을 활성화 한다.)   
          *  **void AttachMagazine()** : 총기에 탄창을 붙이는 기능(손에 있는 탄창을 비활성화하고 원래 무기에 붙어있는 탄창을 활성화한다. 애니메이션 파라미터 초기화 후 UI를 업데이트 해준다.)

* **RayCastWeapon1.cs**  
  * **Main Function**  : **ActiveWeapon.cs**의 Update()에서 호출하는 함수  
    * **void UpdateWeapon(float deltaTime, Vector3 target)** : 

## 개선 사항
* 애니메이션 이벤트 함수 의존성
  * 애니메이션 이벤트 함수 로 스킬 딜레이 및 연계를 제어하는데 의도치않게 모션이 캔슬되는경우(ex 벽으로 대시) 다음 스킬이 안나가는 버그 발생   
    * 시전 시간이 끝나면 관련 변수 초기화로 임시 해결  
* Enemy가 상속성이 없어서 상호작용에 있어서 복잡한 코드 작성  
  *  K_PlayerFire.cs  
      ```
      if (hit.collider.tag == "Enemy")
      {
          N_Enemy enemy = hit.transform.GetComponent<N_Enemy>();
          if (enemy != null)
              {
                  enemy.AddDamage(damage);

              }
          K_Enemy kenemy = hit.transform.GetComponent<K_Enemy>();
          if (kenemy != null)
          {
              kenemy.AddDamage(damage);

          }

          N_MiniBoss1 miniBoss1 = hit.transform.GetComponent<N_MiniBoss1>();
          if (miniBoss1 != null)
              {
                  miniBoss1.AddDamage(damage);
              }

          N_MiniBoss2 miniBoss2 = hit.transform.GetComponent<N_MiniBoss2>();
          if (miniBoss2 != null)
          {
              miniBoss2.AddDamage(damage);
          }
          N_Boss boss = hit.transform.GetComponent<N_Boss>();
          if (boss != null)
              {
                  boss.AddDamage(damage);
              }


      }
      ```  
      * 체력과 데미지를 가하는 함수는 모든 Enemy의 공통된 기능이기 때문에 상속성을 사용하면 보다 간결한 코드가 될 것으로 보임. 

## License

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

## Requirement

[Unity 2021.3.5f1 (LTS)](https://unity.cn/release-notes/lts/2020/2020.3.4f1)

