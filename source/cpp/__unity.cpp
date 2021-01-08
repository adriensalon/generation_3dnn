#include "GPROC.h"




void exporter_geometrie(
	callback_geometrie callback,
	const geometrie& g)
{
	log("message : (methode) generation::exporter");

	auto nombre_vertices = (int)g.positions.size();
	auto nombre_triangles = (int)g.indices.size();

	std::vector<float> positions_x = std::vector<float>(nombre_vertices);
	std::vector<float> positions_y = std::vector<float>(nombre_vertices);
	std::vector<float> positions_z = std::vector<float>(nombre_vertices);
	std::vector<float> normales_x = std::vector<float>(nombre_vertices);
	std::vector<float> normales_y = std::vector<float>(nombre_vertices);
	std::vector<float> normales_z = std::vector<float>(nombre_vertices);
	std::vector<float> uvs_x = std::vector<float>(nombre_vertices);
	std::vector<float> uvs_y = std::vector<float>(nombre_vertices);
	for (auto i = 0; i < nombre_vertices; i++)
	{
		positions_x[i] = g.positions[i].x;
		positions_y[i] = g.positions[i].y;
		positions_z[i] = g.positions[i].z;
		normales_x[i] = g.normales[i].x;
		normales_y[i] = g.normales[i].y;
		normales_z[i] = g.normales[i].z;
		uvs_x[i] = g.uvs[i].x;
		uvs_y[i] = g.uvs[i].y;
	}
	std::vector<int> indices = std::vector<int>((int)3 * nombre_triangles);
	for (auto i = 0; i < nombre_triangles; i++)
	{
		indices[(int)3 * i] = g.indices[i].x;
		indices[(int)3 * i + 1] = g.indices[i].y;
		indices[(int)3 * i + 2] = g.indices[i].z;
	}

	callback(
		nombre_vertices,
		3 * nombre_triangles,
		&positions_x[0], &positions_y[0], &positions_z[0],
		&normales_x[0], &normales_y[0], &normales_z[0],
		&uvs_x[0], &uvs_y[0],
		&indices[0]);
	
	return;
}










extern "C"
{
	geometrie_procedurale_api
	void unity_creer_projet(
		char* url_dossier,
		callback_log cb_log)
	{
		projet_actuel.cb_log = cb_log;
		construction_console_log();

		organisation::creer_projet(
			url_dossier);

		construction_matlab();
		construction_contexte_lua();
		return;
	}

	geometrie_procedurale_api
	void unity_charger_projet(
		char* url_dossier,
		callback_log cb_log)
	{
		projet_actuel.cb_log = cb_log;
		construction_console_log();

		organisation::charger_projet(
			url_dossier);

		construction_matlab();
		construction_contexte_lua();
		return;
	}

	geometrie_procedurale_api
	void unity_obtenir_noms_scripts(
		callback_liste_string callback)
{
	std::string s;
	std::vector<int> c;
	int l = 0;

	for (auto i = 0; i < projet_actuel.noms_scripts.size(); i++)
	{
		std::string str = projet_actuel.noms_scripts[i];
		s += str;
		c.push_back((int)str.size());
		l += (int)str.size();
	}

	callback(l, (char*)s.data(), c.size(), &c[0]);
	return;
}

	geometrie_procedurale_api
		void unity_obtenir_noms_categories(
			callback_liste_string callback)
	{
		std::string s;
		std::vector<int> c;
		int l = 0;

		for (auto i = 0; i < projet_actuel.noms_categories.size(); i++)
		{
			std::string str = projet_actuel.noms_categories[i];
			s += str;
			c.push_back((int)str.size());
			l += (int)str.size();
		}

		callback(l, (char*)s.data(), c.size(), &c[0]);
		return;
	}

	geometrie_procedurale_api
		void unity_obtenir_noms_geometries(
			std::string categorie,
			callback_liste_string callback)
	{
		std::string s;
		std::vector<int> c;
		int l = 0;

		for (auto i = 0; i < projet_actuel.noms_schemas[categorie].size(); i++)
		{
			std::string str = projet_actuel.noms_schemas[categorie][i];
			s += str;
			c.push_back((int)str.size());
			l += (int)str.size();
		}

		callback(l, (char*)s.data(), c.size(), &c[0]);
		return;
	}

	geometrie_procedurale_api
		void unity_obtenir_noms_ias(
			std::string categorie,
			callback_liste_string callback)
	{
		std::string s;
		std::vector<int> c;
		int l = 0;

		for (auto i = 0; i < projet_actuel.noms_ias[categorie].size(); i++)
		{
			std::string str = projet_actuel.noms_ias[categorie][i];
			s += str;
			c.push_back((int)str.size());
			l += (int)str.size();
		}

		callback(l, (char*)s.data(), c.size(), &c[0]);
		return;
	}

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
			float* ajustement_taille)
	{
		auto g = geometrie();

		g.positions.resize(nombre_vertices);
		g.normales.resize(nombre_vertices);
		g.uvs.resize(nombre_vertices);
		for (auto i = 0; i < nombre_vertices; i++)
		{
			g.positions[i] = reel3(positions_x[i], positions_y[i], positions_z[i]);
			g.normales[i] = reel3(normales_x[i], normales_y[i], normales_z[i]);
			g.uvs[i] = reel2(uv_x[i], uv_y[i]);
		}

		g.indices.resize(nombre_triangles);
		for (auto i = 0; i < nombre_triangles; i++)
		{
			g.indices[i] = entier3(indices_x[i], indices_y[i], indices_z[i]);
		}

		auto v_ajustement = std::vector<float>(ajustement_dim_x, ajustement_dim_x + 3);
		auto v_taille = std::vector<float>(ajustement_taille, ajustement_taille + 3);
		auto p = parametres_calcul_geometrie();
		p.ajustement_debut_x = v_ajustement[0];
		p.ajustement_extension_x = v_ajustement[1];
		p.ajustement_fin_x = v_ajustement[2];
		p.boiteX = v_taille[0];
		p.boiteY = v_taille[1];
		p.boiteZ = v_taille[2];

		projet_actuel.sauvegarde_chargee_editeur = preparation::calculer_geometrie(
			g,
			p);

		return;
	}

	geometrie_procedurale_api
		void unity_sauvegarder_geometrie(
			char* categorie,
			char* nom_fichier)
	{
		preparation::sauvegarder_geometrie(
			categorie,
			nom_fichier);


		return;
	}

	geometrie_procedurale_api
		void unity_generer_geometrie_editeur(
			callback_geometrie callback,
			float* taille)
	{
		auto v_taille = std::vector<float>(taille, taille + 3);

		auto g = generation::generer_geometrie(
			projet_actuel.sauvegarde_chargee_editeur,
			v_taille[0]);

		sauvegarder_fichier_off("C:/Users/adri/Desktop/final.off", g);
		exporter_geometrie(callback, g);

		return;
	}

	geometrie_procedurale_api
		void unity_generer_geometrie(
			char* nom_sauvegarde,
			callback_geometrie callback,
			float* taille)
	{
		auto v_taille = std::vector<float>(taille, taille + 3);

		auto g = generation::generer_geometrie(
			projet_actuel.sauvegardes_chargees[nom_sauvegarde],
			v_taille[0]);
		
		exporter_geometrie(callback, g);

		return;
	}

	// ===========================================

	geometrie_procedurale_api
		void unity_commencer_entrainement_texture(
			char* nom_categorie,
			char* nom_ia,
			int utiliser_gpu,
			int continuer)
	{
		auto p = parametres_entrainement_texture();
		p.utiliser_gpu = utiliser_gpu > 0;
		p.continuer = continuer > 0;

		preparation::commencer_entrainement_texture(
			nom_categorie,
			nom_ia,
			p);

		return;
	}

	geometrie_procedurale_api
		void unity_obtenir_resultats_entrainement_texture(
			callback_resultats_entrainement_texture cb)
	{
		auto r = preparation::obtenir_resultats_entrainement_texture();

		if (r.nouveau_tick)
		{
			cb(
				r.epoch,
				r.gLoss,
				r.dLoss,
				r.lossL1,
				r.ganLoss);
		}
		return;
	}

	geometrie_procedurale_api
		void unity_arreter_entrainement_texture()
	{
		preparation::interrompre_entrainement_texture();

		return;
	}

	geometrie_procedurale_api
		void unity_generer_texture(
			char* nom_categorie,
			char* nom_ia,
			char* url_label,
			char* url_target,
			int utiliser_gpu)
	{
		generation::generer_texture(
			nom_categorie,
			nom_ia,
			url_label,
			url_target,
			utiliser_gpu > 0);

		return;
	}

	geometrie_procedurale_api
	void unity_executer_script(
		char* nom_script,
		callback_photographie cb_photo,
		callback_geometrie cb_geo)
	{
		generation::executer_script(
			nom_script,
			cb_photo,
			cb_geo);

		return;
	}
}