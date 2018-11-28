TARGET = qsqloci

HEADERS += $$PWD/qsql_oci_p.h
SOURCES += $$PWD/qsql_oci.cpp $$PWD/main.cpp

#QMAKE_USE += oci
#QMAKE_LFLAGS += oci.dll

QMAKE_LFLAGS += oci.lib

darwin:QMAKE_LFLAGS += -Wl,-flat_namespace,-U,_environ

OTHER_FILES += oci.json

PLUGIN_CLASS_NAME = QOCIDriverPlugin
include(../qsqldriverbase.pri)
