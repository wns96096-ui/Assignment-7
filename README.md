# Assignment #7 — Unreal Engine Modules & Plugins

UE 5.8 Third Person C++ 프로젝트에서 추가 프로젝트 모듈과 재사용 가능한 런타임 플러그인을 구성한 과제입니다.

## 구현 환경

- Unreal Engine 5.8.1
- Visual Studio / Development Editor / Win64
- 프로젝트: `MyModuleAndPlugin`
- 추가 프로젝트 모듈: `Test`
- 프로젝트 플러그인: `Temporary`

## 구조

```text
MyModuleAndPlugin
├─ MyModuleAndPlugin.uproject
├─ Source
│  ├─ MyModuleAndPlugin
│  │  └─ MyModuleAndPlugin.Build.cs
│  └─ Test
│     ├─ Test.Build.cs
│     ├─ Public
│     │  ├─ Test.h
│     │  └─ TestActor.h
│     └─ Private
│        ├─ Test.cpp
│        └─ TestActor.cpp
└─ Plugins
   └─ Temporary
      ├─ Temporary.uplugin
      ├─ Content
      └─ Source
         └─ Temporary
            ├─ Temporary.Build.cs
            ├─ Public
            │  ├─ Temporary.h
            │  └─ TemporaryCharacterData.h
            └─ Private
               ├─ Temporary.cpp
               └─ TemporaryCharacterData.cpp
```

## 파일별 역할

| 파일 | 역할 |
|---|---|
| `*.Build.cs` | 모듈의 컴파일 설정과 다른 모듈에 대한 Public/Private 의존성을 선언합니다. |
| `*.Target.cs` | Game 또는 Editor 프로그램을 만들 때 포함할 프로젝트 모듈을 선택합니다. |
| `.uproject` | 프로젝트 모듈의 타입·로딩 단계와 사용할 플러그인을 선언합니다. |
| `.uplugin` | 플러그인의 메타데이터, 콘텐츠 포함 여부, 내부 모듈의 타입·로딩 단계를 선언합니다. |

Visual Studio 솔루션은 코드 탐색용으로 생성되는 결과물이며, Unreal Build Tool은 `Target.cs`와 `Build.cs`를 기준으로 빌드합니다.

## 의존 방향

```text
MyModuleAndPlugin ──Private──> Test ──Public──> Core / CoreUObject / Engine

MyModuleAndPlugin ──Public───> Temporary ──Public──> Core / CoreUObject
Temporary ──X──> MyModuleAndPlugin
```

- `TestActor`는 주 게임 모듈의 `.cpp` 구현에서만 사용하므로 `Test`는 Private dependency입니다.
- `UTemporaryCharacterData`는 주 게임 모듈의 공개 Character 헤더에 `UPROPERTY` 타입으로 나타나므로 `Temporary`는 Public dependency입니다.
- `Temporary`는 특정 프로젝트 클래스를 참조하지 않습니다. 따라서 다른 프로젝트로 옮겨도 `MyModuleAndPlugin` 코드가 필요하지 않습니다.

## Test 모듈

- `.uproject`: `Runtime`, `PreDefault`
- Game 및 Editor `Target.cs`: `ExtraModuleNames`에 `Test` 포함
- `IMPLEMENT_MODULE(FDefaultModuleImpl, Test)`로 기본 모듈 등록
- `TEST_API ATestActor`를 Public 헤더로 공개
- 주 Character의 `BeginPlay()`에서 `ATestActor` 스폰

실행 로그:

```text
LogTemp: Warning: [Test] ATestActor::BeginPlay
LogMyModuleAndPlugin: Spawned TestActor: TestActor_0
```

## Temporary 플러그인

- `.uplugin`: `CanContainContent: true`
- 내부 모듈: `Runtime`, `Default`
- `.uproject`: `Temporary` 활성화
- `StartupModule()`과 `ShutdownModule()`에서 수명주기 로그 출력
- `TEMPORARY_API UTemporaryCharacterData`에 캐릭터 이름과 최대 체력 저장
- Character가 `UPROPERTY(Transient) TObjectPtr`로 객체를 보관하고 `NewObject(..., this)`로 생성

실행 로그:

```text
LogPluginManager: Mounting Project plugin Temporary
LogModuleManager: InternalLoadLibrary: UnrealEditor-Temporary.dll
LogTemp: Warning: [Temporary] StartupModule
LogMyModuleAndPlugin: [TemporaryData] Name=Temporary Hero MaxHealth=100
LogTemp: Warning: [Temporary] ShutdownModule
```

`StartupModule()`은 모듈이 메모리에 로드될 때 실행되고, `BeginPlay()`는 게임 월드에서 플레이가 시작될 때 액터 인스턴스마다 실행됩니다.

## 빌드 및 실행

1. `.uproject`를 우클릭하고 Visual Studio 프로젝트 파일을 생성합니다.
2. Visual Studio에서 `Development Editor / Win64`를 선택합니다.
3. 솔루션을 빌드합니다.
4. 에디터를 실행하고 플레이합니다.
5. 출력 로그에서 `Temporary`, `Test`, `TemporaryData`를 검색합니다.

2026-08-19에 프로젝트와 플러그인의 `Binaries`·`Intermediate`를 제거한 뒤 UE 5.8에서 프로젝트 파일을 재생성하고 `MyModuleAndPluginEditor Win64 Development` 클린 빌드를 완료했습니다.

Git에는 `Source`, `Config`, `Content`, `Plugins`, `.uproject`를 포함하고, 재생성 가능한 `Binaries`, `Intermediate`, `Saved`, `DerivedDataCache`, IDE 생성 파일은 제외합니다.

## 오류 회고

구현 중 서로 다른 모듈에 같은 이름의 `TestActor.h`가 생성되어 다음 UHT 오류가 발생했습니다.

```text
Two headers with the same name is not allowed.
```

주 게임 모듈의 `MYMODULEANDPLUGIN_API ATestActor` 중복본을 제거하고, 과제 목적에 맞는 `Test` 모듈의 `TEST_API ATestActor`를 정본으로 유지했습니다. 자세한 분류와 재발 방지 기준은 [TIL.md](TIL.md)에 기록했습니다.
