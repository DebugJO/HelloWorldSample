```
bun create tauri-app@latest

Project name: my-tauri-react (원하는 이름)
Choose which language to use for your frontend: TypeScript / JavaScript : JavaScript 선택
Choose your package manager: bun
Choose your UI template: React

cd my-tauri-react
bun install


src-tauri/capabilities/default.json

{
  "$schema": "../gen/schemas/desktop-capability.json",
  "identifier": "default",
  "description": "default capability",
  "windows": [
    "main"
  ],
  "permissions": [
    "core:default",
    "core:window:allow-start-dragging",
    "core:window:allow-minimize",
    "core:window:allow-maximize",
    "core:window:allow-toggle-maximize",
    "core:window:allow-close"
  ]
}

src-tauri/tauri.conf.json

"windows": [
  {
    "title": "My App",
    "width": 800,
    "height": 600,
    "decorations": false,  
    "transparent": false,  
    "fullscreen": false,
    "resizable": true
  }
]


src/App.css

:root {
  color-scheme: light !important;
}

html, body, #root {
  background-color: #ffffff !important;
  color: #333333 !important;
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
  font-family: sans-serif;
}

.main-content {
  padding-top: 32px;
  box-sizing: border-box;
}

src/Titlebar.jsx

import { getCurrentWindow } from '@tauri-apps/api/window';
import './Titlebar.css'; // 타이틀바 전용 스타일

export default function Titlebar() {
  const minimize = (e) => {
    e.stopPropagation();
    getCurrentWindow().minimize();
  };

  const maximize = (e) => {
    e.stopPropagation();
    getCurrentWindow().toggleMaximize();
  };

  const close = (e) => {
    e.stopPropagation();
    getCurrentWindow().close();
  };

  return (
    <div className="titlebar" data-tauri-drag-region>
      <div className="title">My React App</div>
      <div className="window-controls">
        <button className="control-btn minimize" onClick={minimize}>➖</button>
        <button className="control-btn maximize" onClick={maximize}>🔳</button>
        <button className="control-btn close" onClick={close}>❌</button>
      </div>
    </div>
  );
}

src/Titlebar.css

.titlebar {
  height: 32px;
  background: #f3f3f3;
  color: #333333;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  padding: 0 10px;
  user-select: none;
  z-index: 9999;
  border-bottom: 1px solid #e0e0e0;
}

.title {
  font-size: 12px;
  pointer-events: none;
}

.window-controls {
  display: flex;
  gap: 5px;
}

.control-btn {
  background: transparent;
  border: none;
  width: 28px;
  height: 28px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 10px;
  color: #333333;
  border-radius: 4px;
  -webkit-app-region: no-drag; 
}

.control-btn:hover {
  background: rgba(0, 0, 0, 0.1);
}

.control-btn.close:hover {
  background: #ff4d4f;
  color: white;
}


src/App.jsx

import { useState } from "react";
import { invoke } from "@tauri-apps/api/core";
import Titlebar from "./Titlebar"; 
import "./App.css";

function App() {
  const [greetMsg, setGreetMsg] = useState("");
  const [name, setName] = useState("");

  async function greet() {
    setGreetMsg(await invoke("greet", { name }));
  }

  return (
    <>
      <Titlebar />

      {/* 본문 컨텐츠 메인 영역 */}
      <div className="main-content">
        <main className="container">
          <h1>Welcome to Tauri + React</h1>

          <form
            className="row"
            onSubmit={(e) => {
              e.preventDefault();
              greet();
            }}
          >
            <input
              id="greet-input"
              onChange={(e) => setName(e.currentTarget.value)}
              placeholder="Enter a name..."
            />
            <button type="submit">Greet</button>
          </form>
          <p>{greetMsg}</p>
        </main>
      </div>
    </>
  );
}

export default App;
```
