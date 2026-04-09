### VS2022 vcpkg drogon 예제

개발툴 : CLion + vs2022 툴체인

**CMakeLists.txt**

```cmake
cmake_minimum_required(VERSION 4.2)
set(CMAKE_PREFIX_PATH "C:/vcpkg/installed/x64-windows" CACHE PATH "")
set(CMAKE_TOOLCHAIN_FILE "C:/vcpkg/scripts/buildsystems/vcpkg.cmake" CACHE FILEPATH "")
set(X_VCPKG_APPLOCAL_DEPS ON CACHE BOOL "")

project(drogon_test)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

find_package(Drogon CONFIG REQUIRED)

add_executable(${PROJECT_NAME} main.cpp)

target_link_libraries(${PROJECT_NAME} PRIVATE Drogon::Drogon)

if (WIN32)
    set(TARGET_DIR "$<TARGET_FILE_DIR:${PROJECT_NAME}>")
    set(DEPLOY_DIR "${TARGET_DIR}/deploy")

    add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E make_directory "${DEPLOY_DIR}"
            COMMAND ${CMAKE_COMMAND} -E copy "$<TARGET_FILE:${PROJECT_NAME}>" "${DEPLOY_DIR}/"
            COMMAND cmd /c "copy /y \"${TARGET_DIR}\\*.dll\" \"${DEPLOY_DIR}\\\""
            COMMENT "프로젝트 [${PROJECT_NAME}]의 모든 필수 DLL을 deploy 폴더로 복사 중..."
    )
endif ()
```

**man.cpp**

```cpp
#include <drogon/drogon.h>

int main() {
    drogon::app().registerHandler(
        "/", [](const drogon::HttpRequestPtr &req,
                std::function<void (const drogon::HttpResponsePtr &)> &&callback) {
            const auto resp = drogon::HttpResponse::newHttpResponse();
            resp->setStatusCode(drogon::k200OK);
            resp->setContentTypeCode(drogon::CT_TEXT_HTML);
            resp->setBody("<h1>Hello, Drogon World!</h1><p>Installation Success!</p>");
            callback(resp);
        }
    );

    LOG_INFO << "Server running on http://127.0.0.1:8080";
    drogon::app().addListener("0.0.0.0", 8080);
    drogon::app().run();

    return 0;
}
```
