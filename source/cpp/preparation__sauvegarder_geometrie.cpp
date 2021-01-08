#include "GPROC.h"

void creer_categorie(
    const std::string& nom_categorie)
{
    std::string url_dossier_categorie = projet_actuel.url_dossier + "data/" + nom_categorie;
    if (std::filesystem::exists(url_dossier_categorie))
    {
        log("message : impossible de creer la categorie");
        return;
    }

    std::filesystem::create_directory(url_dossier_categorie);
    if (!std::filesystem::exists(url_dossier_categorie))
    {
        log("message : impossible de creer la categorie - dossier categorie");
        return;
    }
    projet_actuel.noms_categories.push_back(nom_categorie);

    std::string url_dossier_motifs = url_dossier_categorie + "/geometries";
    std::filesystem::create_directory(url_dossier_motifs);
    if (!std::filesystem::exists(url_dossier_motifs))
    {
        log("message : impossible de creer la categorie = dossier schemas");
        return;
    }

    std::string url_dossier_apprentissage = url_dossier_categorie + "/apprentissage";
    std::filesystem::create_directory(url_dossier_apprentissage);
    if (!std::filesystem::exists(url_dossier_apprentissage))
    {
        log("message : impossible de creer la categorie - dossier apprentissage");
        return;
    }

    std::string url_dossier_label = url_dossier_apprentissage + "/textures_label";
    std::filesystem::create_directory(url_dossier_label);
    if (!std::filesystem::exists(url_dossier_label))
    {
        log("message : impossible de creer la categorie - dossier label");
        return;
    }
    std::string url_dossier_target = url_dossier_apprentissage + "/textures_target";
    std::filesystem::create_directory(url_dossier_target);
    if (!std::filesystem::exists(url_dossier_target))
    {
        log("message : impossible de creer la categorie - dossier target");
        return;
    }
    std::string url_dossier_ia = url_dossier_apprentissage + "/ias";
    std::filesystem::create_directory(url_dossier_ia);
    if (!std::filesystem::exists(url_dossier_ia))
    {
        log("message : impossible de creer la categorie - dossier ias");
        return;
    }
    return;
}






void preparation::sauvegarder_geometrie(
    const std::string nom_categorie,
	const std::string nom_fichier)
{
    demarrer_profilage("sauvegarder_geometrie");


    sauvegarde_geometrie s = projet_actuel.sauvegarde_chargee_editeur;

    bool contient = false;
    for (auto i = 0; i < (int)projet_actuel.noms_categories.size(); i++)
    {
        if (projet_actuel.noms_categories[i] == nom_categorie)
        {
            contient = true;
            break;
        }
    }

    if (!contient)
    {
        creer_categorie(nom_categorie);
    }

    std::string url(projet_actuel.url_dossier + "data/" + nom_categorie + "/geometries/" + nom_fichier);
    std::ofstream stream_fichier(url);

    stream_fichier << s.ajustement_debut_x << "\n";
    stream_fichier << s.ajustement_extension_x << "\n";
    stream_fichier << s.ajustement_fin_x << "\n";

    auto a_nombreVertices = (int)s.a.positions.size();
    auto b_nombreVertices = (int)s.b.positions.size();
    auto c_nombreVertices = (int)s.c.positions.size();
    auto d_nombreVertices = (int)s.d.positions.size();
    stream_fichier << a_nombreVertices << "\n";
    stream_fichier << b_nombreVertices << "\n";
    stream_fichier << c_nombreVertices << "\n";
    stream_fichier << d_nombreVertices << "\n";

    auto a_nombreTriangles = (int)s.a.indices.size();
    auto b_nombreTriangles = (int)s.b.indices.size();
    auto c_nombreTriangles = (int)s.c.indices.size();
    auto d_nombreTriangles = (int)s.d.indices.size();
    stream_fichier << a_nombreTriangles << "\n";
    stream_fichier << b_nombreTriangles << "\n";
    stream_fichier << c_nombreTriangles << "\n";
    stream_fichier << d_nombreTriangles << "\n";

    for (auto iVertex = 0; iVertex < a_nombreVertices; iVertex++)
    {
        stream_fichier << s.a.positions[iVertex].x << "\n";
        stream_fichier << s.a.positions[iVertex].y << "\n";
        stream_fichier << s.a.positions[iVertex].z << "\n";
    }
    for (auto iTriangle = 0; iTriangle < a_nombreTriangles; iTriangle++)
    {
        stream_fichier << s.a.indices[iTriangle].x << "\n";
        stream_fichier << s.a.indices[iTriangle].y << "\n";
        stream_fichier << s.a.indices[iTriangle].z << "\n";
    }

    for (auto iVertex = 0; iVertex < b_nombreVertices; iVertex++)
    {
        stream_fichier << s.b.positions[iVertex].x << "\n";
        stream_fichier << s.b.positions[iVertex].y << "\n";
        stream_fichier << s.b.positions[iVertex].z << "\n";
    }
    for (auto iTriangle = 0; iTriangle < b_nombreTriangles; iTriangle++)
    {
        stream_fichier << s.b.indices[iTriangle].x << "\n";
        stream_fichier << s.b.indices[iTriangle].y << "\n";
        stream_fichier << s.b.indices[iTriangle].z << "\n";
    }

    for (auto iVertex = 0; iVertex < c_nombreVertices; iVertex++)
    {
        stream_fichier << s.c.positions[iVertex].x << "\n";
        stream_fichier << s.c.positions[iVertex].y << "\n";
        stream_fichier << s.c.positions[iVertex].z << "\n";
    }
    for (auto iTriangle = 0; iTriangle < c_nombreTriangles; iTriangle++)
    {
        stream_fichier << s.c.indices[iTriangle].x << "\n";
        stream_fichier << s.c.indices[iTriangle].y << "\n";
        stream_fichier << s.c.indices[iTriangle].z << "\n";
    }

    for (auto iVertex = 0; iVertex < d_nombreVertices; iVertex++)
    {
        stream_fichier << s.d.positions[iVertex].x << "\n";
        stream_fichier << s.d.positions[iVertex].y << "\n";
        stream_fichier << s.d.positions[iVertex].z << "\n";
    }
    for (auto iTriangle = 0; iTriangle < d_nombreTriangles; iTriangle++)
    {
        stream_fichier << s.d.indices[iTriangle].x << "\n";
        stream_fichier << s.d.indices[iTriangle].y << "\n";
        stream_fichier << s.d.indices[iTriangle].z << "\n";
    }

    auto ab_nombreVerticesDecoupe = (int)s.a_ligne_decoupe_x_b.size();
    auto bc_nombreVerticesDecoupe = (int)s.b_ligne_decoupe_x_b.size();
    auto cd_nombreVerticesDecoupe = (int)s.c_ligne_decoupe_x_b.size();
    stream_fichier << ab_nombreVerticesDecoupe << "\n";
    stream_fichier << bc_nombreVerticesDecoupe << "\n";
    stream_fichier << cd_nombreVerticesDecoupe << "\n";

    for (auto i = 0; i < ab_nombreVerticesDecoupe; i++)
    {
        stream_fichier << s.a_ligne_decoupe_x_b[i] << "\n";
        stream_fichier << s.b_ligne_decoupe_x_a[i] << "\n";
    }
    for (auto i = 0; i < bc_nombreVerticesDecoupe; i++)
    {
        stream_fichier << s.b_ligne_decoupe_x_b[i] << "\n";
        stream_fichier << s.c_ligne_decoupe_x_a[i] << "\n";
    }
    for (auto i = 0; i < cd_nombreVerticesDecoupe; i++)
    {
        stream_fichier << s.c_ligne_decoupe_x_b[i] << "\n";
        stream_fichier << s.d_ligne_decoupe_x_a[i] << "\n";
    }

    stream_fichier.close();

    arreter_profilage("sauvegarder_geometrie");


    projet_actuel.sauvegardes_chargees[nom_fichier] = s;
    organisation::charger_projet(projet_actuel.url_dossier);
    return;
}