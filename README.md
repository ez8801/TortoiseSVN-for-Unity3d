# TortoiseSVN for Unity3d

A Unity custom package that shows SVN commit history inside the Unity Editor.
It shells out to the TortoiseSVN command line client (`svn.exe`) and renders the
parsed log in a `MultiColumnListView`-based editor window.

| | |
| --- | --- |
| Package name | `com.ez.tortoisesvn` |
| Version | 1.0.0 |
| Unity | 2022.3 or newer |
| License | MIT |
| Platform | Windows |

---

## Requirements

- Unity **2022.3** or newer (UI Toolkit `MultiColumnListView` / `TwoPaneSplitView` are used).
- [TortoiseSVN](https://tortoisesvn.net/) installed **with the `command line client tools` option enabled**.
  That option is *not* selected by default in the TortoiseSVN installer - without it there is no
  `svn.exe` and this package cannot run.
- Default client path: `C:/Program Files/TortoiseSVN/bin/svn.exe`

## Installation

### Install via git URL

Edit your Unity project's `Packages/manifest.json` and add this repository as a dependency:

``` json
{
  "dependencies": {
    "com.ez.tortoisesvn": "https://github.com/ez8801/TortoiseSVN-for-Unity3d.git"
  }
}
```

Or in the Unity Editor: **Window > Package Manager > + > Add package from git URL...**
and paste `https://github.com/ez8801/TortoiseSVN-for-Unity3d.git`.

## Getting Started

1. Open **TortoiseSVN > Show Log**.
2. On first run the window shows `Settings not found.` - press **Create Settings**.
   This creates `Assets/Settings/TortoiseSVN Settings.asset`.
3. Select that asset and fill in:
   - **Svn Client Path** - path to `svn.exe`. Left empty, it falls back to
     `C:/Program Files/TortoiseSVN/bin/svn.exe`.
   - **Repository Url** - the repository (or sub-path) whose log you want to read.
4. Reopen **TortoiseSVN > Show Log**. The log is fetched when the window is enabled.

The settings asset can also be created manually via
**Assets > Create > TortoiseSVN > Settings**, but it must live at
`Assets/Settings/TortoiseSVN Settings.asset` - that path is hard-coded in
`Settings.Default.AssetPath`.

## Menu Items

| Menu | Window | Purpose |
| --- | --- | --- |
| `TortoiseSVN/Show Log` | `LogMessagesWindow` | Fetches and displays commit history. |
| `TortoiseSVN/Path` | `PathWindow` | Path inspection utility - reports whether a typed path resolves to an existing directory or file, and shows the resolved absolute forms. |

## Log Window

The window is a vertical split:

- **Top** - a column list: `Revision`, `Author`, `Date`, `Message`.
- **Bottom** - a read-only text field showing the full commit message of the selected row,
  plus a field for the affected paths.

Under the hood it runs:

```
svn log -v --limit 15 <RepositoryUrl>
```

so the window shows the **15 most recent revisions**.

## Behaviour & Limitations

These are current characteristics of v1.0.0. Read them before filing a bug:

- **Windows only.** The default client path is a Windows path and stdout is decoded as
  code page **949 (EUC-KR)**. Non-Korean, non-Windows setups will need `SvnCommand` adjusted.
- **Fixed limit of 15 revisions.** `--limit 15` is hard-coded in `LogMessagesWindow.SvnLog()`.
- **Changed paths are filtered.** `SvnLogParser` only keeps affected paths containing
  `.json` or `.xlsx`. Other file types are parsed but dropped from `ChangedPath`.
- **1 second process timeout.** `proc.WaitForExit(1000)` - a slow repository or a server
  that prompts for credentials will yield truncated or empty output.
- **No credential handling.** Authentication must already be cached by your SVN client;
  the process runs with `CreateNoWindow = true` and cannot answer a prompt.
- **Log is fetched once**, in `OnEnable`. There is no refresh button - close and
  reopen the window to re-query.

## Project Layout

```
Editor/
  LogMessagesWindow.cs      # main window, svn.exe invocation
  Settings.cs               # ScriptableObject: SvnClientPath, RepositoryUrl
  SettingsHelper.cs
  View/
    SvnLogView.cs           # MultiColumnListView + split panes
    PathWindow.cs           # path inspection utility
  PackageResources/
    TortoiseSVN.uss         # window styles
Runtime/
  Data/SvnLog.cs            # Revision / Author / Date / ContextMessage / ChangedPath
  Util/SvnLogParser.cs      # parses `svn log -v` stdout
```

Assemblies: `EZ.TortoiseSVN` (runtime) and `EZ.TortoiseSVN.Editor` (editor).

## License

MIT - see [LICENSE.md](LICENSE.md).

---

# TortoiseSVN for Unity3d (한국어)

Unity 에디터 안에서 SVN 커밋 히스토리를 볼 수 있는 Unity 커스텀 패키지입니다.
TortoiseSVN 커맨드라인 클라이언트(`svn.exe`)를 실행하고, 그 출력을 파싱해
`MultiColumnListView` 기반 에디터 윈도우에 표시합니다.

| | |
| --- | --- |
| 패키지 이름 | `com.ez.tortoisesvn` |
| 버전 | 1.0.0 |
| Unity | 2022.3 이상 |
| 라이선스 | MIT |
| 플랫폼 | Windows |

---

## 요구 사항

- Unity **2022.3** 이상 (UI Toolkit의 `MultiColumnListView` / `TwoPaneSplitView` 사용).
- [TortoiseSVN](https://tortoisesvn.net/) 설치 시 **`command line client tools` 옵션을 반드시 포함**해야 합니다.
  이 옵션은 TortoiseSVN 설치 관리자에서 기본으로 선택되어 있지 **않습니다**.
  포함하지 않으면 `svn.exe`가 없어 이 패키지가 동작하지 않습니다.
- 기본 클라이언트 경로: `C:/Program Files/TortoiseSVN/bin/svn.exe`

## 설치

### git URL로 설치

Unity 프로젝트의 `Packages/manifest.json`에 이 저장소를 의존성으로 추가합니다.

``` json
{
  "dependencies": {
    "com.ez.tortoisesvn": "https://github.com/ez8801/TortoiseSVN-for-Unity3d.git"
  }
}
```

또는 Unity 에디터에서 **Window > Package Manager > + > Add package from git URL...** 를 선택하고
`https://github.com/ez8801/TortoiseSVN-for-Unity3d.git` 를 붙여넣습니다.

## 시작하기

1. **TortoiseSVN > Show Log** 메뉴를 엽니다.
2. 최초 실행 시 `Settings not found.` 가 표시됩니다. **Create Settings** 버튼을 누르면
   `Assets/Settings/TortoiseSVN Settings.asset` 이 생성됩니다.
3. 생성된 에셋을 선택하고 다음 값을 입력합니다.
   - **Svn Client Path** - `svn.exe` 경로. 비워 두면
     `C:/Program Files/TortoiseSVN/bin/svn.exe` 가 사용됩니다.
   - **Repository Url** - 로그를 조회할 저장소(또는 하위 경로) URL.
4. **TortoiseSVN > Show Log** 를 다시 엽니다. 로그는 윈도우가 활성화될 때 조회됩니다.

설정 에셋은 **Assets > Create > TortoiseSVN > Settings** 로 직접 만들 수도 있지만,
경로가 `Settings.Default.AssetPath` 에 하드코딩되어 있으므로 반드시
`Assets/Settings/TortoiseSVN Settings.asset` 위치여야 합니다.

## 메뉴

| 메뉴 | 윈도우 | 용도 |
| --- | --- | --- |
| `TortoiseSVN/Show Log` | `LogMessagesWindow` | 커밋 히스토리 조회 및 표시. |
| `TortoiseSVN/Path` | `PathWindow` | 경로 확인 도구. 입력한 경로가 실제 디렉터리/파일인지 판별하고, 변환된 절대 경로를 보여줍니다. |

## 로그 윈도우

윈도우는 상하 분할 구조입니다.

- **상단** - `Revision`, `Author`, `Date`, `Message` 컬럼 목록.
- **하단** - 선택한 행의 전체 커밋 메시지를 보여주는 읽기 전용 텍스트 필드와
  변경 경로(affected paths) 필드.

내부적으로 다음 명령을 실행합니다.

```
svn log -v --limit 15 <RepositoryUrl>
```

따라서 윈도우에는 **최근 15개 리비전**만 표시됩니다.

## 동작 특성 및 제약

v1.0.0의 현재 동작 특성입니다. 버그로 판단하기 전에 먼저 확인하세요.

- **Windows 전용.** 기본 클라이언트 경로가 Windows 경로이며, 표준 출력을
  코드 페이지 **949(EUC-KR)** 로 디코딩합니다. 한국어/Windows 환경이 아니면
  `SvnCommand` 수정이 필요합니다.
- **리비전 15개 고정.** `--limit 15` 가 `LogMessagesWindow.SvnLog()` 에 하드코딩되어 있습니다.
- **변경 경로가 필터링됨.** `SvnLogParser` 는 `.json` 또는 `.xlsx` 를 포함한 경로만 남깁니다.
  다른 확장자는 파싱은 되지만 `ChangedPath` 에서 제외됩니다.
- **프로세스 타임아웃 1초.** `proc.WaitForExit(1000)` - 응답이 느린 저장소나
  인증을 요구하는 서버에서는 출력이 잘리거나 비어 있을 수 있습니다.
- **인증 처리 없음.** SVN 클라이언트에 인증 정보가 이미 캐시되어 있어야 합니다.
  프로세스는 `CreateNoWindow = true` 로 실행되어 인증 프롬프트에 응답할 수 없습니다.
- **로그는 `OnEnable` 시점에 한 번만 조회.** 새로고침 버튼이 없으므로
  다시 조회하려면 윈도우를 닫았다가 다시 열어야 합니다.

## 프로젝트 구조

```
Editor/
  LogMessagesWindow.cs      # 메인 윈도우, svn.exe 실행
  Settings.cs               # ScriptableObject: SvnClientPath, RepositoryUrl
  SettingsHelper.cs
  View/
    SvnLogView.cs           # MultiColumnListView + 분할 패널
    PathWindow.cs           # 경로 확인 도구
  PackageResources/
    TortoiseSVN.uss         # 윈도우 스타일
Runtime/
  Data/SvnLog.cs            # Revision / Author / Date / ContextMessage / ChangedPath
  Util/SvnLogParser.cs      # `svn log -v` 출력 파서
```

어셈블리: `EZ.TortoiseSVN`(런타임), `EZ.TortoiseSVN.Editor`(에디터).

## 라이선스

MIT - [LICENSE.md](LICENSE.md) 참고.
