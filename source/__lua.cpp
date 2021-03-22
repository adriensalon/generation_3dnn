#include <generation_procedurale.hpp>

#include <selene.h>


static sel::State contexte_lua;

#define script_reel double
#define script_entier int

static std::vector<geometrie> geometries_generees;
static std::vector<std::string> urls_textures_2d;
static std::vector<std::string> noms_ias;




void generer(
	const char* nom_categorie,
	const char* nom_geometrie,
	const char* nom_ia,
	const script_reel position_x, const script_reel position_y, const script_reel position_z,
	const script_reel rotation_x, const script_reel rotation_y, const script_reel rotation_z,
	const script_reel taille_x, const script_reel taille_y, const script_reel taille_z)
{
	auto g = generation::generer_geometrie(projet_actuel.sauvegardes_chargees[nom_geometrie], taille_x);
	
	transformer_geometrie(
		g,
		reel3((float)position_x, (float)position_y, (float)position_z),
		reel3((float)rotation_x, (float)rotation_y, (float)rotation_z),
		reel3((float)taille_x, (float)taille_y, (float)taille_z));

	geometries_generees.push_back(g);
	urls_textures_2d.push_back(projet_actuel.url_dossier + "/data/" + nom_categorie + "/label/" + nom_geometrie + ".jpg");
	noms_ias.push_back(nom_ia);

	return;
}




void construction_contexte_lua()
{
	contexte_lua["generer_geometrie"] = &generer;
	return;
}





void generation::executer_script(
	std::string nom_script,
	callback_photographie cb_photo,
	callback_geometrie cb_geo)
{
	contexte_lua.Load(projet_actuel.url_dossier + "scripts/" + nom_script + ".lua");
	contexte_lua("");

	for (auto i = 0; i < geometries_generees.size(); i++)
	{/*
		void** p = cb_geo();
		void* ptr_vertices = p[0];
		void* ptr_triangles = p[1];
		assigner_buffer_vertices(
			geometries_generees[i], 
			ptr_vertices);
		assigner_buffer_triangles(
			geometries_generees[i],
			ptr_triangles);*/

		// degueu ici
		std::string url_texture_predict_label = projet_actuel.url_dossier + "data/";// +nom_cate
		std::string url_texture_predict_target = projet_actuel.url_dossier + "data/";// +nom_cate
		cb_photo(
			url_texture_predict_label.size(),
			url_texture_predict_target.size(),
			url_texture_predict_label.data(),
			url_texture_predict_target.data());

		/*generation::generer_texture(
			noms_ias[i],
			url_texture_predict_label,
			url_texture_predict_target,
			true);*/
	}

	return;
}