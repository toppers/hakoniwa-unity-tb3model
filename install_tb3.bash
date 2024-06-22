#!/bin/bash

if [ -d plugin/plugin-srcs ]
then
    :
else
    echo "ERROR: can not find submodule plugin"
    echo "git pull"
    echo "git submodule update --init --recursive"
    exit 1
fi

echo "INFO: DOWNLOADING TB3 MODEL"

if [ -f tb3/Assets/Model/Hakoniwa/Robots/BurgerBot_Model/TB3Burgar_01.fbx ]
then
    echo "INFO: TB3 MODEL IS ALREADY INSTALLED."
else
    wget https://github.com/toppers/hakoniwa-unity-tb3model/releases/download/model-v1.0.0/TB3Burgar_01.fbx
    mv TB3Burgar_01.fbx tb3/Assets/Model/Hakoniwa/Robots/BurgerBot_Model/

    echo "INFO: COPYING TB3Assets to hakoniwa plugin(plugin/plugin-srcs/Assets/)"
    cp -rp tb3/Assets/* plugin/plugin-srcs/Assets/
    cp tb3/lidar2d_tb3_spec.json plugin/plugin-srcs/
fi


echo "TB3 DONE"
exit 0
