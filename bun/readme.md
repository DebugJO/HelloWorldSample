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
