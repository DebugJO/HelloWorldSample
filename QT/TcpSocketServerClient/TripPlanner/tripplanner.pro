TEMPLATE = app

QT += gui network widgets

CONFIG += c++11
CONFIG -= app_bundle

HEADERS = tripplanner.h

SOURCES = main.cpp \
          tripplanner.cpp

FORMS = tripplanner.ui
