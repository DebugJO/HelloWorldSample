## WSL2 개발환경 설정(

기본적으로 windows terminal 설치

### 1. WSL 설치(terminal에서 실행)
```bat
wsl --install
wsl --install -d <배포판>
```

기본으로 Ubuntu가 설치됨. 설치 후 재부팅
* Microsoft Store에서 Ubuntu 앱 업데이트 완료 -> 터미널에서 ubuntu 선택(최초 아이디/패스워드)

```bash
wsl -l -v #상태보기 
wsl -l -o #설치할 수 있는 공식 배포판 리스트
wsl -t Ubuntu #종료
wsl --unregister <배포판> #배포판 삭제
wsl --set-default-version 2 # 기본버전(옵션)
```
### 터미널 설정

* 폰트 설정 (Hack폰트) : https://github.com/source-foundry/Hack-windows-installer 또는 https://github.com/powerline/fonts 에서 선택
* 터미널 -> 설정 ->  Ubuntu -> 모양(글꼴)에서 선택
* 시작 디렉터리 설정 : Ubuntu -> 일반 -> 디렉터리를 시작하는 중 : \\wsl$\Ubuntu\home\<사용자>\
* Dark+ 테마 추가 ->  Ubuntu -> 모양(색구성표)에서 선택
```json
{
    "background": "#0E0E0E",
    "black": "#000000",
    "blue": "#2472C8",
    "brightBlack": "#666666",
    "brightBlue": "#3B8EEA",
    "brightCyan": "#29B8DB",
    "brightGreen": "#23D18B",
    "brightPurple": "#D670D6",
    "brightRed": "#F14C4C",
    "brightWhite": "#E5E5E5",
    "brightYellow": "#F5F543",
    "cursorColor": "#FFFFFF",
    "cyan": "#11A8CD",
    "foreground": "#CCCCCC",
    "green": "#0DBC79",
    "name": "Dark+",
    "purple": "#BC3FBC",
    "red": "#CD3131",
    "selectionBackground": "#3A3D41",
    "white": "#E5E5E5",
    "yellow": "#E5E510"
}
```

### Ubuntu zsh구성

* Git, zsh 설치
```bash
sudo apt update
sudo apt install git zsh
```
* Oh my zsh 설치 및 구성
```bash
sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)"
```

* PlugIn 추가
```bash
git clone https://github.com/zsh-users/zsh-autosuggestions ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-autosuggestions

git clone https://github.com/zsh-users/zsh-syntax-highlighting.git ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-syntax-highlighting
```

* 테마구성(사용자 디렉터리에서 작업)
```bash
# vi .zshrc

export ZSH="$HOME/.oh-my-zsh"
ZSH_THEME="agnoster"
plugins=(git zsh-autosuggestions zsh-syntax-highlighting)
source $ZSH/oh-my-zsh.sh
prompt_context() {}
LS_COLORS=$LS_COLORS:'ow=1;34:' ; export LS_COLORS
export PATH="/home/msjo/.local/bin":$PATH
```

### vi 설정
```bash
# vi .vimrc

if has("syntax")
        syntax on
endif
set hlsearch
set nu
set autoindent
set ts=4
set sts=4
set cindent
set laststatus=2
set shiftwidth=4
set showmatch
set smartcase
set smarttab
set smartindent
set ruler
set fileencodings=utf8,euc-kr
```

### 추가 패키지 설치
```bash
sudo apt install bat
sudo apt install tree
sudo apt install fonts-nanum
```

### 기본 개발 패키지 설치
```bash
sudo apt install build-essential
sudo apt install clang-12 --install-suggests
sudo apt install lldb-12
sudo apt install clang-format-12
sudo apt install cmake
sudo apt install openjdk-17-jdk
```

### Rust 설치
``` bash
curl https://sh.rustup.rs -sSf | sh
source $HOME/.cargo/env
```

### dotnet 설치
```bash
wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-6.0
```

build-essential, gcc, clang cmake java dotnet mariadb, mongodb, bat
