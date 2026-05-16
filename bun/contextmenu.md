### 마우스 우측 버튼 해제

```
방법 1

src/main.tsx

import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'

if (import.meta.env.PROD) {
  window.addEventListener('contextmenu', (e) => e.preventDefault());
}

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)

방법 2

src-tauri/src/lib.rs

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .on_page_load(|window, _payload| {
            #[cfg(not(debug_assertions))]
            let _ = window.eval("window.addEventListener('contextmenu', (e) => e.preventDefault());");
        })
        .plugin(tauri_plugin_shell::init())
        .invoke_handler(tauri::generate_handler![greet])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

커스텀 컨텍스트 메뉴

src/ContextMenu.css

.custom-context-menu {
  position: absolute;
  background-color: #ffffff;
  border: 1px solid #cccccc;
  border-radius: 6px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.15);
  padding: 5px 0;
  min-width: 150px;
  z-index: 10000; 
}

.context-menu-item {
  padding: 8px 14px;
  font-size: 13px;
  color: #333333;
  cursor: pointer;
  display: flex;
  align-items: center;
  user-select: none;
}

.context-menu-item:hover {
  background-color: #f5f5f5;
  color: #000000;
}

.context-menu-divider {
  height: 1px;
  background-color: #eeeeee;
  margin: 4px 0;
}

src/App.tsx

import React, { useState, useEffect } from "react";
import { invoke } from "@tauri-apps/api/core";
import Titlebar from "./Titlebar";
import "./App.css";
import "./ContextMenu.css";

interface MenuPosition {
  x: number;
  y: number;
}

function App() {
  const [greetMsg, setGreetMsg] = useState<string>("");
  const [name, setName] = useState<string>("");

  const [showMenu, setShowMenu] = useState<boolean>(false);
  const [menuPos, setMenuPos] = useState<MenuPosition>({ x: 0, y: 0 });

  const handleContextMenu = (e: React.MouseEvent): void => {
    e.preventDefault();   
    setMenuPos({ x: e.pageX, y: e.pageY });
    setShowMenu(true);
  };

  useEffect(() => {
    const closeMenu = () => setShowMenu(false);
    window.addEventListener("click", closeMenu);
    return () => window.removeEventListener("click", closeMenu);
  }, []);

  async function greet(): Promise<void> {
    const response = await invoke<string>("greet", { name });
    setGreetMsg(response);
  }

  const handleMenuAction = (action: string) => {
    alert(`${action} 기능이 클릭되었습니다!`);
    // 여기에 실제 원하는 기능을 넣기 (예: 복사, 붙여넣기, 뒤로가기 등)
  };

  return (
    <div onContextMenu={handleContextMenu} style={{ width: "100%", height: "100%" }}>
      <Titlebar />

      {showMenu && (
        <div
          className="custom-context-menu"
          style={{ top: `${menuPos.y}px`, left: `${menuPos.x}px` }}
        >
          <div className="context-menu-item" onClick={() => handleMenuAction("새로고침")}>🔄 새로고침</div>
          <div className="context-menu-item" onClick={() => handleMenuAction("링크 복사")}>🔗 링크 복사</div>
          <div className="context-menu-divider"></div>
          <div className="context-menu-item" onClick={() => handleMenuAction("설정")}>⚙️ 설정 항목</div>
        </div>
      )}

      {/* 메인 콘텐츠 영역 */}
      <div className="main-content">
        <main className="container">
          <h1>Welcome to Tauri + React (TS)</h1>
          <p>이 화면 어디든 우클릭을 해보세요!</p>

          <form
            className="row"
            onSubmit={(e: React.FormEvent<HTMLFormElement>) => {
              e.preventDefault();
              greet();
            }}
          >
            <input
              id="greet-input"
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setName(e.currentTarget.value)}
              placeholder="Enter a name..."
            />
            <button type="submit">Greet</button>
          </form>
          <p>{greetMsg}</p>
        </main>
      </div>
    </div>
  );
}

export default App;

```
