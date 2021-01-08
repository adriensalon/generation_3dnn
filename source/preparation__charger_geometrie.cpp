#include "GPROC.h"

sauvegarde_geometrie preparation::charger_geometrie(
	const std::string categorie,
	const std::string nom_schema)
{
    demarrer_profilage("charger_geometrie");

    std::string s = projet_actuel.url_dossier + "data/" + categorie + "/geometries/" + nom_schema;
    std::fstream streamFichier(s);

    auto sauvegarde = sauvegarde_geometrie();

    if (!streamFichier.good())
    {
        log("erreur : (bibliotheque) le fichier motif n'a pas pu etre lu");
        return sauvegarde;
    }

    streamFichier >> sauvegarde.ajustement_debut_x;
    streamFichier >> sauvegarde.ajustement_extension_x;
    streamFichier >> sauvegarde.ajustement_fin_x;

    auto a_nombreVertices = 0;
    auto b_nombreVertices = 0;
    auto c_nombreVertices = 0;
    auto d_nombreVertices = 0;
    streamFichier >> a_nombreVertices;
    streamFichier >> b_nombreVertices;
    streamFichier >> c_nombreVertices;
    streamFichier >> d_nombreVertices;
    auto a_nombreTriangles = 0;
    auto b_nombreTriangles = 0;
    auto c_nombreTriangles = 0;
    auto d_nombreTriangles = 0;
    streamFichier >> a_nombreTriangles;
    streamFichier >> b_nombreTriangles;
    streamFichier >> c_nombreTriangles;
    streamFichier >> d_nombreTriangles;

    sauvegarde.a.positions.resize(a_nombreVertices);
    sauvegarde.a.indices.resize(a_nombreTriangles);
    for (auto a_iVertex = 0; a_iVertex < a_nombreVertices; a_iVertex++)
    {
        streamFichier >> sauvegarde.a.positions[a_iVertex].x;
        streamFichier >> sauvegarde.a.positions[a_iVertex].y;
        streamFichier >> sauvegarde.a.positions[a_iVertex].z;
    }
    for (auto a_iTriangle = 0; a_iTriangle < a_nombreTriangles; a_iTriangle++)
    {
        streamFichier >> sauvegarde.a.indices[a_iTriangle].x;
        streamFichier >> sauvegarde.a.indices[a_iTriangle].y;
        streamFichier >> sauvegarde.a.indices[a_iTriangle].z;
    }

    sauvegarde.b.positions.resize(b_nombreVertices);
    sauvegarde.b.indices.resize(b_nombreTriangles);
    for (auto b_iVertex = 0; b_iVertex < b_nombreVertices; b_iVertex++)
    {
        streamFichier >> sauvegarde.b.positions[b_iVertex].x;
        streamFichier >> sauvegarde.b.positions[b_iVertex].y;
        streamFichier >> sauvegarde.b.positions[b_iVertex].z;
    }
    for (auto b_iTriangle = 0; b_iTriangle < b_nombreTriangles; b_iTriangle++)
    {
        streamFichier >> sauvegarde.b.indices[b_iTriangle].x;
        streamFichier >> sauvegarde.b.indices[b_iTriangle].y;
        streamFichier >> sauvegarde.b.indices[b_iTriangle].z;
    }

    sauvegarde.c.positions.resize(c_nombreVertices);
    sauvegarde.c.indices.resize(c_nombreTriangles);
    for (auto c_iVertex = 0; c_iVertex < c_nombreVertices; c_iVertex++)
    {
        streamFichier >> sauvegarde.c.positions[c_iVertex].x;
        streamFichier >> sauvegarde.c.positions[c_iVertex].y;
        streamFichier >> sauvegarde.c.positions[c_iVertex].z;
    }
    for (auto c_iTriangle = 0; c_iTriangle < c_nombreTriangles; c_iTriangle++)
    {
        streamFichier >> sauvegarde.c.indices[c_iTriangle].x;
        streamFichier >> sauvegarde.c.indices[c_iTriangle].y;
        streamFichier >> sauvegarde.c.indices[c_iTriangle].z;
    }

    sauvegarde.d.positions.resize(d_nombreVertices);
    sauvegarde.d.indices.resize(d_nombreTriangles);
    for (auto d_iVertex = 0; d_iVertex < d_nombreVertices; d_iVertex++)
    {
        streamFichier >> sauvegarde.d.positions[d_iVertex].x;
        streamFichier >> sauvegarde.d.positions[d_iVertex].y;
        streamFichier >> sauvegarde.d.positions[d_iVertex].z;
    }
    for (auto d_iTriangle = 0; d_iTriangle < d_nombreTriangles; d_iTriangle++)
    {
        streamFichier >> sauvegarde.d.indices[d_iTriangle].x;
        streamFichier >> sauvegarde.d.indices[d_iTriangle].y;
        streamFichier >> sauvegarde.d.indices[d_iTriangle].z;
    }

    auto ab_nombreVerticesDecoupe = 0;
    auto bc_nombreVerticesDecoupe = 0;
    auto cd_nombreVerticesDecoupe = 0;
    streamFichier >> ab_nombreVerticesDecoupe;
    streamFichier >> bc_nombreVerticesDecoupe;
    streamFichier >> cd_nombreVerticesDecoupe;
    sauvegarde.a_ligne_decoupe_x_b.resize(ab_nombreVerticesDecoupe);
    sauvegarde.b_ligne_decoupe_x_a.resize(ab_nombreVerticesDecoupe);
    sauvegarde.b_ligne_decoupe_x_b.resize(bc_nombreVerticesDecoupe);
    sauvegarde.c_ligne_decoupe_x_a.resize(bc_nombreVerticesDecoupe);
    sauvegarde.c_ligne_decoupe_x_b.resize(cd_nombreVerticesDecoupe);
    sauvegarde.d_ligne_decoupe_x_a.resize(cd_nombreVerticesDecoupe);
    for (auto i = 0; i < ab_nombreVerticesDecoupe; i++)
    {
        streamFichier >> sauvegarde.a_ligne_decoupe_x_b[i];
        streamFichier >> sauvegarde.b_ligne_decoupe_x_a[i];
    }
    for (auto i = 0; i < bc_nombreVerticesDecoupe; i++)
    {
        streamFichier >> sauvegarde.b_ligne_decoupe_x_b[i];
        streamFichier >> sauvegarde.c_ligne_decoupe_x_a[i];
    }
    for (auto i = 0; i < cd_nombreVerticesDecoupe; i++)
    {
        streamFichier >> sauvegarde.c_ligne_decoupe_x_b[i];
        streamFichier >> sauvegarde.d_ligne_decoupe_x_a[i];
    }

    streamFichier.close();

    arreter_profilage("charger_geometrie");
	return sauvegarde;
}