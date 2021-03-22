#include <generation_procedurale.hpp>

void organisation::charger_projet(
	const std::string& url_dossier)
{
	demarrer_profilage("charger_projet");

	projet_actuel.url_dossier = url_dossier;

	if (!projet_actuel.url_dossier.ends_with('/')) projet_actuel.url_dossier += "/";
	if (!std::filesystem::exists(projet_actuel.url_dossier))
	{
		log("message : impossible de charger le projet");
		return;
	}

	projet_actuel.noms_scripts.resize(0);
	for (const auto& e_script : std::filesystem::directory_iterator(projet_actuel.url_dossier + "scripts"))
	{
		std::string script_actuel = e_script.path().filename().generic_string();
		projet_actuel.noms_scripts.push_back(script_actuel);
	}

	projet_actuel.noms_categories.resize(0);
	for (const auto& e_categorie : std::filesystem::directory_iterator(projet_actuel.url_dossier + "data"))
	{
		std::string nom_categorie_actuelle = e_categorie.path().filename().generic_string();
		projet_actuel.noms_categories.push_back(nom_categorie_actuelle);
		
		std::string url_dossier_geometries = projet_actuel.url_dossier + "data/" + nom_categorie_actuelle + "/geometries";
		projet_actuel.noms_schemas[nom_categorie_actuelle].resize(0);
		for (const auto& e_geometrie : std::filesystem::directory_iterator(url_dossier_geometries))
		{
			std::string nom_geometrie_actuelle = e_geometrie.path().filename().generic_string();
			projet_actuel.noms_schemas[nom_categorie_actuelle].push_back(nom_geometrie_actuelle);

			auto sauvegarde = preparation::charger_geometrie(
				nom_categorie_actuelle,
				nom_geometrie_actuelle);

			projet_actuel.sauvegardes_chargees[nom_geometrie_actuelle] = sauvegarde;
		}

		std::string url_dossier_ias = projet_actuel.url_dossier + "data/" + nom_categorie_actuelle + "/apprentissage/ias/";
		projet_actuel.noms_ias[nom_categorie_actuelle].resize(0);
		for (const auto& e_ia : std::filesystem::directory_iterator(url_dossier_ias))
		{
			std::string ia_actuelle = e_ia.path().generic_string();
			projet_actuel.noms_ias[nom_categorie_actuelle].push_back(ia_actuelle);
		}
	}

	arreter_profilage("charger_projet");
	return;
}