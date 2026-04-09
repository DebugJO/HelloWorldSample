cmake_minimum_required(VERSION 4.2)
project(msys2_qt)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_PREFIX_PATH "C:/msys64/ucrt64")

find_package(Qt6 REQUIRED COMPONENTS Core Sql)
find_package(SQLite3 REQUIRED)

add_executable(${PROJECT_NAME}
        main.cpp
        db_raii.h
)

target_link_libraries(${PROJECT_NAME} PRIVATE
        Qt6::Core
        Qt6::Sql
        SQLite::SQLite3
)

if (WIN32)
    if (TARGET Qt6::qmake)
        get_target_property(_qmake_executable Qt6::qmake IMPORTED_LOCATION)
        get_filename_component(_qt_bin_dir "${_qmake_executable}" DIRECTORY)
        find_program(WINDEPLOYQT_EXECUTABLE windeployqt HINTS "${_qt_bin_dir}")
    endif ()

    if (WINDEPLOYQT_EXECUTABLE)
        set(DEPLOY_DIR "${CMAKE_BINARY_DIR}/deploy")
        set(DEPLOY_MODE "--release")
        set(MINIMIZE_FLAGS "--no-translations" "--no-opengl-sw")

        add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD
                COMMAND "${WINDEPLOYQT_EXECUTABLE}" ${DEPLOY_MODE} --compiler-runtime --no-translations --verbose 0 "$<TARGET_FILE:${PROJECT_NAME}>"
                COMMAND ${CMAKE_COMMAND} -E rm -rf "${DEPLOY_DIR}"
                COMMAND ${CMAKE_COMMAND} -E make_directory "${DEPLOY_DIR}"
                COMMAND ${CMAKE_COMMAND} -E copy "$<TARGET_FILE:${PROJECT_NAME}>" "${DEPLOY_DIR}/"
                COMMAND ${CMAKE_COMMAND} -E copy_if_different
                "C:/msys64/ucrt64/bin/libsqlite3-0.dll"
                "C:/msys64/ucrt64/bin/libstdc++-6.dll"
                "C:/msys64/ucrt64/bin/libgcc_s_seh-1.dll"
                "C:/msys64/ucrt64/bin/libwinpthread-1.dll"
                "C:/msys64/ucrt64/bin/libicuuc78.dll"
                "C:/msys64/ucrt64/bin/libicuin78.dll"
                "C:/msys64/ucrt64/bin/libicudt78.dll"
                "C:/msys64/ucrt64/bin/libb2-1.dll"
                "C:/msys64/ucrt64/bin/libdouble-conversion.dll"
                "C:/msys64/ucrt64/bin/zlib1.dll"
                "C:/msys64/ucrt64/bin/libpcre2-16-0.dll"
                "C:/msys64/ucrt64/bin/libzstd.dll"
                "${DEPLOY_DIR}/"

                COMMAND "${WINDEPLOYQT_EXECUTABLE}" ${DEPLOY_MODE} --compiler-runtime ${MINIMIZE_FLAGS} --verbose 1 "${DEPLOY_DIR}/$<TARGET_FILE_NAME:${PROJECT_NAME}>"
                COMMENT "windeployqt 및 MSYS2 필수 DLL 강제 수집 완료"
        )
    endif ()
endif ()
