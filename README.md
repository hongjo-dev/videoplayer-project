# TestProject

## 📋 프로젝트 개요
이 프로젝트는 대학교 2학년 때 처음으로 Visual Studio와 C#을 사용하여 만든 간단한 **미디어 플레이어** 애플리케이션입니다. 당시 프로그래밍 지식이 부족하여 모든 로직과 UI 코드를 `MainForm`에 몰아서 작성했기 때문에 구조적으로 비효율적일 수 있습니다.  

현재 프로젝트는 주로 **Bunifu UI**와 **LAV Filters**를 사용하며, 미디어 재생과 관련된 기능을 제공합니다. 프로젝트는 학습 목적으로 만들어졌으며, 구조적인 개선이 필요합니다.

---

## 🛠️ 주요 기능
1. **미디어 재생**:
   - LAV Filters를 사용하여 MP4와 같은 미디어 파일을 재생합니다.
2. **UI 인터페이스**:
   - Bunifu UI를 활용하여 현대적이고 깔끔한 사용자 인터페이스를 제공합니다.
3. **볼륨 조절 및 상태 표시**:
   - 사용자 정의 슬라이더를 사용해 볼륨 조절 및 현재 상태를 표시합니다.

---

## 🔧 설치 방법

### 1. **필수 요구사항**
- **Windows 10 이상**
- **Visual Studio 2022** (또는 호환 가능한 버전)
- **.NET Framework 4.8**
- **LAV Filters**

### 2. **Bunifu UI 설치**
- Bunifu는 상용 라이브러리이므로 별도로 구매하여 설치해야 합니다.
- 설치 후 프로젝트의 **NuGet 패키지 관리자** 또는 로컬 참조로 추가하세요.

### 3. **LAV Filters 설치**
- [LAV Filters 공식 웹사이트](https://github.com/Nevcairiel/LAVFilters)에서 설치 파일을 다운로드합니다.
- 다운로드 후 설치하면 프로젝트에서 MP4, MKV 등 다양한 미디어 파일 형식을 지원합니다.

---

## 📂 프로젝트 구조
프로젝트는 모든 로직이 `MainForm`에 집중되어 있으며, 추가적인 분할이 되어 있지 않습니다.

### 주요 파일 설명
- `MainForm.cs`: 모든 로직과 UI 이벤트 핸들러가 포함된 메인 파일.
- `MainForm.Designer.cs`: Visual Studio 디자이너에서 자동 생성된 UI 코드.
- `Program.cs`: 프로그램의 진입점.
- `MediaStatus.cs`: 미디어 재생 상태를 관리하는 추가 클래스.
- `Properties/`: 프로젝트 설정 및 리소스 관리 폴더.
  - `AssemblyInfo.cs`: 어셈블리 메타데이터.
  - `Resources.resx`: 리소스 파일(이미지, 문자열 등).
  - `Settings.settings`: 응용 프로그램 설정.

---

## 💻 실행 방법
1. Visual Studio에서 솔루션 파일(`TestProject.sln`)을 엽니다.
2. NuGet 패키지 복원을 실행합니다.
3. 필요한 라이브러리(Bunifu 및 LAV Filters)가 설치되었는지 확인합니다.
4. `Debug` 또는 `Release` 모드로 빌드 후 실행합니다.

---

## 🌟 개선할 점
1. **코드 구조 개선**:
   - 현재 모든 로직이 `MainForm`에 집중되어 있어 유지보수성이 떨어집니다.
   - 비즈니스 로직과 UI 로직을 분리하여 클래스를 모듈화하는 것이 필요합니다.

2. **에러 핸들링 추가**:
   - 미디어 파일이 지원되지 않을 때의 에러 처리와 사용자 알림 기능 추가.

3. **의존성 관리**:
   - `LAV Filters`와 같은 외부 도구 설치 과정을 자동화하거나, 설치 가이드를 상세히 제공.

4. **UI 개선**:
   - 현재는 Bunifu UI를 사용하지만, 무료 UI 라이브러리로 전환하여 접근성을 개선할 수도 있음.

---

## 📜 라이선스
이 프로젝트는 학습 목적으로 작성되었으며, 상업적 용도로 사용되지 않습니다. 프로젝트에 사용된 **Bunifu UI**와 **LAV Filters**는 각각의 라이선스를 준수해야 합니다.

---

## 🙋‍♂️ 작성자
- **이름**: 안홍조
- **이메일**: hongjo9999@naver.com

---

## 📝 참고
- [Bunifu UI 공식 웹사이트](https://bunifuframework.com/)
- [LAV Filters GitHub 페이지](https://github.com/Nevcairiel/LAVFilters)
