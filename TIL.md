# TIL — 모듈·플러그인 빌드 오류 분류

## 1. UHT 중복 헤더 오류

### 증상

```text
MyModuleAndPluginEditor.uhtmanifest(1):
Two headers with the same name is not allowed.
```

### 분류

C++ 링크나 런타임 모듈 로딩 전 단계인 Unreal Header Tool의 리플렉션 코드 생성 오류입니다.

### 원인

다음 두 위치에 `TestActor.h/.cpp`가 동시에 존재했습니다.

```text
Source/MyModuleAndPlugin/Public/TestActor.h
Source/Test/Public/TestActor.h
```

두 헤더는 파일명뿐 아니라 `ATestActor`라는 같은 반영 클래스도 선언했습니다.

### 해결

과제의 의도는 `Test` 모듈의 클래스를 주 게임 모듈이 사용하는 것이므로 다음 정본을 유지했습니다.

```cpp
class TEST_API ATestActor : public AActor
```

주 게임 모듈에 잘못 생성된 `MYMODULEANDPLUGIN_API` 복사본은 제거했습니다.

### 재발 방지

- C++ 클래스 생성 창에서 대상 모듈과 Public/Private 위치를 확인합니다.
- 빌드 전에 프로젝트 전체에서 같은 헤더명과 `UCLASS`명을 검색합니다.
- API 매크로를 보고 클래스가 실제로 어느 모듈 소속인지 판별합니다.

## 2. 잘못 입력된 C++ 토큰

`TemporaryCharacterData.cpp` 첫 줄에 불필요한 `al` 문자가 들어간 것을 빌드 전에 발견했습니다. 이 문제는 `.cpp` 문법을 해석하는 C++ 컴파일 오류이며 UHT·링크·로딩 오류가 아닙니다.

## 3. 단계별 오류 판별 기준

| 단계 | 대표 증상 | 먼저 확인할 것 |
|---|---|---|
| 빌드 규칙 | `Build.cs` 또는 `Target.cs` 규칙 타입을 찾지 못함 | 파일명, 클래스명, C# 문법 |
| UHT | `generated.h`, 중복 반영 타입·헤더 오류 | `UCLASS`, 파일명, 생성 헤더 위치 |
| C++ 컴파일 | 헤더를 찾지 못함, 문법·타입 오류 | include, Public/Private dependency |
| 링크 | `LNK2019 unresolved external symbol` | API export 매크로, 모듈 의존성 |
| 모듈 로딩 | DLL 누락·비호환, 초기화 실패 | `.uproject`, `.uplugin`, 모듈명, `IMPLEMENT_MODULE` |

## 4. 이번 과제에서 확인한 원칙

- `.uproject` 등록은 모듈 간 C++ 사용 의존성을 대신하지 않습니다.
- `Target.cs`는 빌드할 프로그램의 루트 모듈을 선택합니다.
- 사용하는 모듈의 `Build.cs`에 의존 방향을 선언합니다.
- 다른 모듈에서 클래스를 사용하려면 Public 헤더와 API export 매크로가 필요합니다.
- 플러그인은 특정 프로젝트에 역으로 의존하지 않아야 재사용할 수 있습니다.
- `UObject` 인스턴스는 `UPROPERTY` 참조로 보관해야 가비지 컬렉션에서 안전합니다.
