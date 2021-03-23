cmake_minimum_required(VERSION 3.0.0)
project(generation_procedurale VERSION 0.1.0)

# OPTIONS
set(BUILD "bin/Debug")
set(MATLAB "C:/Program Files/MATLAB/MATLAB Runtime/v98/extern")


# -------------------------------------------------------------

# repertoires
include_directories("include")
include_directories("source")
include_directories(${BUILD})
include_directories(${MATLAB}/include)
link_directories(${BUILD})
link_directories(${MATLAB}/lib/win64/microsoft) # MSVC SPECIFIC

# compile la partie independante
set(SOURCE
    "source/__debug.cpp"
    "source/__structures.cpp"
    "source/texture_procedurale.cpp"
    "source/libgproc.cpp"
    "source/organisation__charger_projet.cpp"
    "source/organisation__creer_projet.cpp"
    "source/preparation__calculer_geometrie.cpp"
    "source/preparation__charger_geometrie.cpp"
    "source/preparation__sauvegarder_geometrie.cpp"
    "source/preparation__commencer_entrainement_texture.cpp"
    "source/preparation__interrompre_entrainement_texture.cpp"
    "source/preparation__obtenir_resultats_entrainement_texture.cpp"
    "source/generation__generer_geometrie.cpp"
    "source/generation__generer_texture.cpp")
add_library(
    generation_procedurale SHARED
    ${SOURCE})

target_compile_features(generation_procedurale PRIVATE cxx_std_20) # C++20 SPECIFIC
set_target_properties(generation_procedurale
    PROPERTIES
    ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin"
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin"
)

# compile + link la partie texture_procedurale
set(SOURCE_MATLAB
    ${CMAKE_SOURCE_DIR}/source/matlab/entrainer_ia.m
    ${CMAKE_SOURCE_DIR}/source/matlab/predire_ia.m)
add_custom_command(
    TARGET generation_procedurale PRE_BUILD
    COMMAND mcc 
                -W lib:texture_procedurale 
                -T link:lib 
                -d ${CMAKE_SOURCE_DIR}/${BUILD}
                -g
                -v
                ${SOURCE_MATLAB}
    DEPENDS ${SOURCE_MATLAB})
target_link_libraries(generation_procedurale "texture_procedurale")
target_link_libraries(generation_procedurale "mclmcrrt")

# nettoyer
add_custom_command(
    TARGET generation_procedurale
    COMMAND cp
                ${BUILD}/texture_procedurale.h
                "source"
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})
add_custom_command(
    TARGET generation_procedurale
    COMMAND rm
                ${BUILD}/readme.txt
                ${BUILD}/mccExcludedFiles.log
                ${BUILD}/requiredMCRProducts.txt
                ${BUILD}/texture_procedurale.lib
                ${BUILD}/texture_procedurale.ilk
                ${BUILD}/texture_procedurale.exp
                ${BUILD}/texture_procedurale.exports
                ${BUILD}/texture_procedurale.c
                ${BUILD}/texture_procedurale.h
                ${BUILD}/texture_procedurale.def

                ${BUILD}/generation_procedurale.exp
                ${BUILD}/generation_procedurale.ilk
                ${BUILD}/generation_procedurale.lib
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})

















# copier plugins unity et ouvrir le projet

add_custom_command(
    TARGET generation_procedurale
    DEPENDS generation_procedurale
    COMMAND cp
                ${BUILD}/texture_procedurale.dll
                "c:/users/adri/projets unity/carte/assets/plugins"
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})
add_custom_command(
    TARGET generation_procedurale
    DEPENDS generation_procedurale
    COMMAND cp
                ${BUILD}/texture_procedurale.pdb
                "c:/users/adri/projets unity/carte/assets/plugins"
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})
add_custom_command(
    TARGET generation_procedurale
    DEPENDS generation_procedurale
    COMMAND cp
                ${BUILD}/generation_procedurale.dll
                "c:/users/adri/projets unity/carte/assets/plugins"
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})
add_custom_command(
    TARGET generation_procedurale
    DEPENDS generation_procedurale
    COMMAND cp
                ${BUILD}/generation_procedurale.pdb
                "c:/users/adri/projets unity/carte/assets/plugins"
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})




# add_custom_command(
#     TARGET generation_procedurale
#     DEPENDS generation_procedurale
#     COMMAND "C:/Program Files/Unity Editors/2019.4.0f1/Editor/Unity.exe" 
#                 -projectPath "c:/users/adri/projets unity/carte"
#     WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})


add_custom_command(
    TARGET generation_procedurale PRE_BUILD
    COMMAND taskkill /IM "C:/Program Files/Unity Editors/2019.4.0f1/Editor/Unity.exe" 
    WORKING_DIRECTORY ${CMAKE_SOURCE_DIR})


