# PianoVR
Ghost Pianist (https://github.com/SeungwooSon225/GhostPianist) 를 활용한 VR 피아노 연주 프로젝트

Ghost Pianist에서 전달한 손 추적 정보, 피아노 건반 입력 정보를 받아와 VR 손 모델과 피아노 모델을 컨트롤 함

## 스크립트 설명 (Assets/Scripts/)
HandModelController.cs: Ghost Pianist에서 전달한 손 추적 정보를 받아와 손 모델을 컨트롤하기 위한 스크립트

PianoKeyController.cs: Ghost Pianist에서 전달한 피아노 건반 입력 정보를 받아와 피아노 모델을 컨트롤하기 위한 스크립트

SynchronizePiano.cs: 실제 MIDI 피아노 위치와 VR 피아노 모델의 위치를 동기화 시키기 위한 스크립트
 
SynchronizePianoEditor.cs: SynchronizePiano.cs의 함수들을 인스펙터 창의 버튼으로 조작하기 위한 스크립트

UDPReceiver.cs: Ghost Pianist에서 전달한 손 추적 정보, 피아노 건반 입력 정보를 받아오기위한 소캣 통신이 구현된 스크립트

