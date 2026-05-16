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

#### dotnet

```cs
using System;
using TauriDotNet; // Tauri-NET 라이브러리

namespace MyTauriApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TauriApp.Builder()
                .RegisterCommand("greet", Greet)
                .Run();
        }

        [TauriCommand]
        public static string Greet(string name)
        {
            return $"Hello, {name}! C# .NET 백엔드";
        }
    }
}
```

```ts
import { invoke } from "@tauri-apps/api/core";

const nameInput = document.querySelector("#name-input") as HTMLInputElement;
const greetBtn = document.querySelector("#greet-btn");
const greetMsg = document.querySelector("#greet-msg");

greetBtn?.addEventListener("click", async () => {
  const response = await invoke<string>("greet", { name: nameInput.value });
  
  if (greetMsg) {
    greetMsg.textContent = response;
  }
});
```


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
