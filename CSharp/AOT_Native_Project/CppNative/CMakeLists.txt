cmake_minimum_required(VERSION 4.2)
project(CppNative)
set(CMAKE_CXX_STANDARD 20)

add_compile_options(/utf-8)
# 정적 컴파일
set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>")
# 동적 컴파일 : dumpbin /dependents CppNative.dll 확인
# set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>DLL")
add_library(CppNative SHARED
        library.cpp
)
set_target_properties(CppNative PROPERTIES OUTPUT_NAME "CppNative" PREFIX "")

# 동적 컴파일 시 사용
#if (CMAKE_BUILD_TYPE STREQUAL "Debug")
#    set(CMAKE_INSTALL_DEBUG_LIBRARIES ON)
#endif ()
#include(InstallRequiredSystemLibraries)
set(DEPLOY_DIR "${CMAKE_SOURCE_DIR}/deploy/$<CONFIG>")
add_custom_command(TARGET CppNative POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E make_directory "${DEPLOY_DIR}"
        COMMAND ${CMAKE_COMMAND} -E copy_if_different $<TARGET_FILE:CppNative> "${DEPLOY_DIR}"
        # 동적 컴파일 시 사용
        #COMMAND ${CMAKE_COMMAND} -E copy_if_different ${CMAKE_INSTALL_SYSTEM_RUNTIME_LIBS} "${DEPLOY_DIR}"
        COMMENT "Deploying CppNative.dll and system dependencies to ${DEPLOY_DIR}"
)