### Tauri

```bash
bun create tauri-app@latest
cd my-tauri-app
bun install
bun tauri dev
bun tauri build
```

|**성능 지표**|**Electron**|**Electrobun**|**Tauri**|
|---|---|---|---|
|**최종 빌드 용량**|130MB ~ 150MB|20MB ~ 30MB|7MB ~ 10MB|
|**메모리(RAM) 점유율**|120MB ~ 180MB|40MB ~ 60MB|30MB ~ 50MB|
|**초기 실행 속도 (Startup)**|느림 (브라우저 로딩 필요)|매우 빠름 (Bun의 초고속 시동)|매우 빠름 (Rust 바이너리 시동)|
|**백엔드 실행 속도**|보통 (Node.js)|빠름 (Bun 런타임)|압도적 (Native Rust 기계어)|

#### rust

```ts
// src/components/Counter.tsx
import { useState } from "react";

export default function Counter() {
  const [count, setCount] = useState(0);

  return (
    <div className="p-4 core-box">
      <p>현재 카운트: {count}</p>
      <button onClick={() => setCount(count + 1)} className="btn-primary">
        증가
      </button>
    </div>
  );
}
```

```rust
import { invoke } from "@tauri-apps/api/core";

// 클릭 시 Rust 백엔드에 파일 저장을 요청하는 함수
async function handleSave() {
  const result = await invoke("save_to_local_disk", { data: "저장할 내용" });
  alert(result);
}
```


### Tauri v2 + React + TS + Vite 스택 요약

|**기술 구성 요소**|**담당 역할 (Role)**|**핵심 강점 (Key Strengths)**|**존재 이유 (Why here?)**|
|---|---|---|---|
|**Tauri v2**<br><br>  <br><br>_(App Shell & Backend)_|• 앱의 외각 틀(윈도우) 형성<br><br>  <br><br>• OS 네이티브 기능 제어<br><br>  <br><br>• Rust 기반 백엔드 로직 처리|• **9MB 수준**의 초경량 용량<br><br>  <br><br>• 매우 적은 메모리(RAM) 점유<br><br>  <br><br>• 강력한 모바일(iOS/Android) 지원|일렉트론의 무거운 크롬 엔진을 걷어내고, 시스템 자원을 최소한으로 쓰는 단단한 뼈대를 제공|
|**React**<br><br>  <br><br>_(UI Library)_|• 화면에 보여지는 UI 컴포넌트 설계<br><br>  <br><br>• 사용자 인터페이스 상태 관리|• 전 세계 1위의 압도적 생태계<br><br>  <br><br>• 검증된 컴포넌트 재사용성<br><br>  <br><br>• 풍부한 UI 라이브러리 활용 가능|가장 많은 오픈소스 자원과 예제를 가지고 있어, 원하는 화면 레이아웃과 기능을 가장 빠르게 구현|
|**TypeScript**<br><br>  <br><br>_(Language)_|• 정적 타입 지정을 통한 코드 안정성 확보|• 컴파일 단계에서 오타/버그 차단<br><br>  <br><br>• 코드 자동 완성 기능 극대화<br><br>  <br><br>• 대규모 프로젝트 유지보수 용이|프론트엔드와 Rust 백엔드 간에 데이터를 주고받을 때, 데이터 규격을 명확히 하여 휴먼 에러를 완벽히 막아줌|
|**Vite**<br><br>  <br><br>_(Build Engine)_|• 초고속 개발 서버 구동<br><br>  <br><br>• 소스코드 실시간 반영 (HMR)<br><br>  <br><br>• 최종 배포용 파일 최적화 빌드|• **0.1초대**의 눈부신 코드 반영 속도<br><br>  <br><br>• 내부 컴파일러(Esbuild)의 압도적 성능<br><br>  <br><br>• 복잡한 설정 없는 쾌적함|CMake나 Webpack 같은 복잡한 빌드 설정에서 벗어나, 저장 즉시 화면이 바뀌는 개발 속도|


### bun ts electron

Electrobun : 하단 문서 참조

```bash
powershell -c "irm bun.sh/install.ps1 | iex"
bun -v
mkdir my-app && cd my-app
bun init
bun index.ts
bun add [라이브러리명]
bun build ./index.ts --compile --outfile my-app.exe
```

```bash
bun create @quick-start/electron my-electron-app
cd my-electron-app
bun install
bun dev
bun run build:win
```
### 예제

**Node.js**

```bash
mkdir node-web-server && cd node-web-server
npm init -y
npm install express
npm install -D typescript ts-node @types/node @types/express
npx tsc --init
npx ts-node index.ts
npx tsc  # index.js 파일이 생성됨
node index.js  # 변환된 파일 실행
# bun에서 실행파일 만들기
bun install # index.ts 가정
bun build ./index.ts --compile --minify --outfile my-node-app.exe
```
```ts
const path = require('path');
const exePath = path.dirname(process.execPath);
const configPath = path.join(exePath, 'config.json');
```

```ts
// index.ts
import express from 'express';
const app = express();

app.get('/', (req, res) => {
  res.json({ message: "Node.js 스타일 서버" });
});

app.listen(3000, () => {
  console.log("Server running on port 3000");
});
```

**Bun**

```ts
// index.ts
const server = Bun.serve({
  port: 3000,
  fetch(request) {
    // URL 경로에 따른 분기 처리
    const url = new URL(request.url);
    if (url.pathname === "/") {
      return Response.json({ message: "Bun 최적화 서버" });
    }
    return new Response("Not Found", { status: 404 });
  },
});

console.log(`🚀 서버가 http://localhost:${server.port} 에서 실행 중입니다.`);
```

```bash
mkdir bun-web && cd bun-web
bun init -y
bun add express
bun add -D @types/express
bun index.ts
bun build ./index.ts --compile --outfile server.exe
```

### Bun Desktop

```bash
# Vanilla + TypeScript
bun create @quick-start/electron my-desktop-app
cd my-desktop-app
bun install
bun dev
bun run build:win
bun run build:mac
bun run build:linux
```

### 일렉트론 + rust(napi-rs)

```bash
npm install -g @napi-rs/cli
napi new native-lib
```

```rs
use napi_derive::napi;

#[napi]
pub fn heavy_computation(input: i32) -> i32 {
    // Rust의 강력한 성능을 활용한 로직
    input * 2 
}
```

```bash
npm run build # rust 코드를 .node 파일로 빌드
```

```ts
# 일렉트론(TypeScript)
import { heavyComputation } from './native-lib'
console.log(heavyComputation(100)); // 200
```

## Electrobun 사용법

```bash
curl -fsSL https://bun.sh/install | bash
```

```bash
mkdir my-app
cd my-app
bunx electrobun init
```

```
my-app/
├── src/
│   ├── index.ts         # 애플리케이션 진입점 (메인 실행 파일)
│   ├── components/      # UI 컴포넌트나 모듈
│   ├── routes/          # 라우팅 관련 코드
│   └── utils/           # 유틸리티 함수
├── public/              # 정적 파일 (이미지, CSS 등)
├── bunfig.toml          # bun 설정 파일
├── electrobun.config.ts # Electrobun 빌드/배포 설정
└── package.json         # 프로젝트 의존성 및 스크립트
```

```bash
bun run build
bun run dev
bun run start
```

**electrobun.config.ts bun 기반 빌드 시스템**

```ts
// electrobun.config.ts
import { defineConfig } from "electrobun";

export default defineConfig({
  entry: "src/index.ts",   // 진입 파일
  outDir: "dist",          // 빌드 결과물 디렉토리
  platform: "node",        // 실행 환경 (node, browser 등)
  target: "esnext",        // 트랜스파일 타겟
  sourcemap: true,         // 디버깅용 소스맵 생성
  minify: true,            // 코드 압축 여부
});
```

**package.json 에서 OS 환경설정**

```json
{
  "scripts": {
    "build": "bun run build",
    "build:win": "bun run build --platform=win",
    "build:mac": "bun run build --platform=darwin",
    "build:linux": "bun run build --platform=linux",
    "dev": "bun run dev",
    "start": "bun run start"
  }
}
```

```bash
bun run build:win
bun run build:mac
bun run build:linux
bun run start
```

**패키지**

```bash
bun add <패키지명>
bun remove <패키지명>
bun update
bun update <패키지명>
```

### Electrobun 예제

```
my-app/
src/
├── index.ts
├── routes/
│   ├── home.tsx
│   ├── about.tsx
│   └── secret.tsx
│   └── components/
├── public/
└── electrobun.config.ts
```

index.ts (진입점)

```ts
import { createApp } from "electrobun";
import Home from "./routes/home";
import Secret from "./routes/secret";

const app = createApp();

// 로그인 화면
app.route("/", Home);

// POST 요청으로 로그인 처리
app.post("/login", (ctx) => {
  const { username, password } = ctx.body;

  // 아이디/비번 체크
  if (username === "admin" && password === "1234") {
    ctx.session.authenticated = true;
    return ctx.redirect("/secret");
  }

  return (
    <div>
      <h1>로그인 실패</h1>
      <a href="/">다시 시도</a>
    </div>
  );
});

// 인증된 사용자만 접근 가능
app.route("/secret", (ctx) => {
  if (!ctx.session?.authenticated) {
    return ctx.redirect("/");
  }
  return Secret();
});

app.listen(3000, () => {
  console.log("Server running at http://localhost:3000");
});
```

home.tsx

```tsx
export default function Home() {
  return (
    <div>
      <h1>로그인 페이지</h1>
      <form method="POST" action="/login">
        <label>
          아이디: <input type="text" name="username" />
        </label>
        <br />
        <label>
          비밀번호: <input type="password" name="password" />
        </label>
        <br />
        <button type="submit">로그인</button>
      </form>
    </div>
  );
}
```

about.tsx

```tsx
export default function About() {
  return (
    <div>
      <h1>About Page</h1>
      <p>누구나 접근 가능한 페이지입니다.</p>
      <a href="/">Home으로</a>
    </div>
  );
}
```

secret.tsx

```tsx
export default function Secret() {
  return (
    <div>
      <h1>🔒 Secret Page</h1>
      <p>로그인한 사용자만 볼 수 있는 페이지입니다.</p>
      <a href="/">Home으로</a>
    </div>
  );
}
```
위의 예제는 정확한 로그인보안 또는 JWT 토큰 기반 인증 추가 필요

**컴포넌트 예제**

```bash
src/
├── index.ts
├── components/
│   ├── Header.tsx
│   └── Footer.tsx
├── routes/
│   ├── home.tsx
│   └── about.tsx
```

```tsx
export default function Header() {
  return (
    <header>
      <h1>My Website</h1>
      <nav>
        <a href="/">Home</a> | <a href="/about">About</a>
      </nav>
    </header>
  );
}
```
```tsx
export default function Footer() {
  return (
    <footer>
      <p>© 2026 My Website</p>
    </footer>
  );
}
```

```tsx
import Header from "../components/Header";
import Footer from "../components/Footer";

export default function Home() {
  return (
    <div>
      <Header />
      <main>
        <h2>Home Page</h2>
        <p>이곳은 기본 진입 화면입니다.</p>
      </main>
      <Footer />
    </div>
  );
}
```

```tsx
import Header from "../components/Header";
import Footer from "../components/Footer";

export default function About() {
  return (
    <div>
      <Header />
      <main>
        <h2>About Page</h2>
        <p>이 화면은 /about 경로에서 표시됩니다.</p>
      </main>
      <Footer />
    </div>
  );
}
```

### 간단한 통신 예시

```ts
// main.ts
import { Electrobun } from 'electrobun';

const app = new Electrobun.App();
const win = new Electrobun.BrowserWindow({
    title: "My Native App",
    url: "views/index.html"
});

// 리액트에서 보낸 'hello' 메시지 받기
win.on('hello', (data) => {
    console.log('React에서 보낸 메시지:', data);
    // 다시 리액트로 응답 보내기
    win.send('reply', { message: '반가워!' });
});
```

```ts
// App.tsx
const handleClick = () => {
    // 메인 프로세스로 메시지 전송
    window.electrobun.send('hello', { content: '안녕 메인!' });
};

// 메인의 응답 대기
window.electrobun.on('reply', (data) => {
    alert(data.message);
});
```

### Electrobun bunx

```bash
mkdir my-electrobun-app
cd my-electrobun-app
bunx electrobun init
bun add react react-dom
bun add -D @types/react @types/react-dom electrobun
```

```tsx
// src/views/App.tsx : 자동생성된 index.html이 참조할 react파일
import React from 'react';
import { createRoot } from 'react-dom/client';

const App = () => (
    <div style={{ background: '#222', color: '#fff', height: '100vh', padding: '20px' }}>
        <h1>Electrobun + React</h1>
        <button onClick={() => window.electrobun.send('hello-from-react', { msg: '안녕!' })}>
            메인으로 메시지 보내기
        </button>
    </div>
);

const root = createRoot(document.getElementById('root')!);
root.render(<App />);
```

```ts
// main.ts : 메인윈도우를 띄우고 react와 통신
import { Electrobun } from 'electrobun';

const app = new Electrobun.App();
const win = new Electrobun.BrowserWindow({
    title: 'My App',
    url: 'src/views/index.html'
});

// React에서 보낸 메시지 수신
win.on('hello-from-react', (data) => {
    console.log('수신 데이터:', data);
});
```
**tsconfig.json**

```json
{
  "compilerOptions": {
    "target": "ESNext",
    "module": "ESNext",
    "moduleResolution": "bundler",
    "jsx": "react-jsx",
    "strict": true,
    "skipLibCheck": true
  },
  "include": ["src/**/*"]
}
```

```bash
bunx electrobun dev
bunx electrobun build
```
