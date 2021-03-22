#include <generation_procedurale.hpp>

void organisation::creer_projet(
	const std::string& url_dossier)
{
	demarrer_profilage("creer_projet");

	projet_actuel.url_dossier = url_dossier;
	if (!projet_actuel.url_dossier.ends_with('/')) projet_actuel.url_dossier += "/";

	if (std::filesystem::exists(projet_actuel.url_dossier))
	{
		log("message : projet existant trouve a cet emplacement");
		return;
	}

	std::filesystem::create_directory(projet_actuel.url_dossier);
	if (!std::filesystem::exists(projet_actuel.url_dossier))
	{
		log("message : impossible de creer le dossier projet");
		return;
	}

	std::string url_dossier_data = projet_actuel.url_dossier + "data/";
	std::filesystem::create_directory(projet_actuel.url_dossier + "data/");
	if (!std::filesystem::exists(projet_actuel.url_dossier))
	{
		log("message : impossible de creer le dossier data");
		return;
	}

	std::string url_dossier_scripts = projet_actuel.url_dossier + "scripts/";
	std::filesystem::create_directory(projet_actuel.url_dossier + "scripts/");
	if (!std::filesystem::exists(projet_actuel.url_dossier))
	{
		log("message : impossible de creer le dossier scripts");
		return;
	}

	arreter_profilage("creer_projet");
	return;
}