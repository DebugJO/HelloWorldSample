### bun ts electron

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
