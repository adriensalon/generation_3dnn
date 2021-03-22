#include <generation_procedurale.hpp>

#define geometrie_procedurale_api __declspec(dllexport)

extern "C"
{
	geometrie_procedurale_api
	void unity_creer_projet(
		char* url_dossier,
		callback_log cb_log);

	geometrie_procedurale_api
	void unity_charger_projet(
		char* url_dossier,
		callback_log cb_log);

	geometrie_procedurale_api
	void unity_obtenir_noms_scripts(
		callback_liste_string callback);

	geometrie_procedurale_api
	void unity_obtenir_noms_categories(
		callback_liste_string callback);

	geometrie_procedurale_api
	void unity_obtenir_noms_geometries(
		std::string categorie,
		callback_liste_string callback);

	geometrie_procedurale_api
	void unity_obtenir_noms_ias(
		std::string categorie,
		callback_liste_string callback);

	// ===========================================

	geometrie_procedurale_api
	void unity_calculer_geometrie(
		int nombre_vertices,
		float* positions_x, float* positions_y, float* positions_z,
		float* normales_x, float* normales_y, float* normales_z,
		float* uv_x, float* uv_y,
		int nombre_triangles,
		int* indices_x, int* indices_y, int* indices_z,
		float* ajustement_dim_x,
		float* ajustement_taille);

	geometrie_procedurale_api
	void unity_sauvegarder_geometrie(
		char* nom_categorie,
		char* nom_fichier);

	geometrie_procedurale_api
		void unity_generer_geometrie_editeur(
			callback_geometrie callback,
			float* taille);

	geometrie_procedurale_api
		void unity_generer_geometrie(
			char* nom_sauvegarde,
			callback_geometrie callback,
			float* taille);

	// ===========================================

	geometrie_procedurale_api
	void unity_commencer_entrainement_texture(
		char* nom_categorie,
		char* nom_ia,
		int utiliser_gpu,
		int continuer);

	geometrie_procedurale_api
	void unity_obtenir_resultats_entrainement_texture(
		callback_resultats_entrainement_texture cb);

	geometrie_procedurale_api
	void unity_arreter_entrainement_texture();

	geometrie_procedurale_api
	void unity_generer_texture(
		char* nom_categorie,
        char* nom_ia,
        char* url_label,
        char* url_target,
        int utiliser_gpu);

//	geometrie_procedurale_api
//		void unity_executer_script(
//			char* nom_script,
//			callback_photographie cb_photo,
//			callback_geometrie cb_geo);
}