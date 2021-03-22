#pragma once


#include <vector>
#include <map>
#include <string>
#include <filesystem>
#include <thread>
#include <atomic>
#include <fstream>
#include <iostream>

typedef void (*callback_log)(
	size_t taille_message,
	void* message);

typedef void (*callback_liste_string)(
	size_t taille_chaine, char* chaine,
	size_t taille_indices, void* indices);

typedef char* (*callback_resultats_entrainement_texture)(
	int epochs,
	float gLoss,
	float dLoss,
	float lossL1,
	float ganLoss);

typedef void (*callback_photographie)(
	size_t taille_url_texture_predict_label,
	size_t taille_url_texture_predict_target,
	char* url_texture_predict_label,
	char* url_texture_predict_target);

typedef void (*callback_geometrie)(
	size_t nombre_vertices,
	size_t nombre_triangles,
	void* positionsX, void* positionsY, void* positionsZ,
	void* normalesX, void* normalesY, void* normalesZ,
	void* uvsX, void* uvsY,
	void* indices);





void construction_matlab();
void destruction_matlab();

void construction_console_log();
void log(std::string message);
void demarrer_profilage(std::string nom_instance);
void arreter_profilage(std::string nom_instance);
void destruction_console_log();

void construction_contexte_lua();














struct logique3
{
	logique3();
	logique3(bool x, bool y, bool z);
	bool x;
	bool y;
	bool z;
};

struct reel2
{
	reel2();
	reel2(float x, float y);
	float x;
	float y;
};

struct reel3
{
	reel3();
	reel3(float x, float y, float z);
	float x;
	float y;
	float z;
};
reel3 operator+(const reel3& a, const reel3& b);
reel3 operator*(const float x, const reel3& a);
reel3 operator*(const reel3& a, const reel3& b);

float distance(const reel3& a, const reel3& b);

struct entier3
{
	entier3();
	entier3(int x, int y, int z);
	int x;
	int y;
	int z;
};
entier3 operator+(const int x, const entier3& a);














struct geometrie
{
	geometrie();

	std::vector<reel3> positions;
	std::vector<reel3> normales;
	std::vector<reel2> uvs;
	std::vector<entier3> indices;
};

void transformer_geometrie(
	geometrie& g,
	const reel3 translation,
	const reel3 rotation,
	const reel3 taille);

geometrie combiner_geometries(
	const std::vector<geometrie>& v);






struct sauvegarde_geometrie
{
	sauvegarde_geometrie();

	std::string categorie;

	// 1D
	float boiteX;
	float boiteY;
	float boiteZ;
	float ajustement_debut_x;
	float ajustement_extension_x;
	float ajustement_fin_x;
	geometrie a;
	geometrie b;
	geometrie c;
	geometrie d;
	std::vector<int> a_ligne_decoupe_x_b;
	std::vector<int> b_ligne_decoupe_x_a;
	std::vector<int> b_ligne_decoupe_x_b;
	std::vector<int> c_ligne_decoupe_x_a;
	std::vector<int> c_ligne_decoupe_x_b;
	std::vector<int> d_ligne_decoupe_x_a;
};














struct parametres_calcul_geometrie
{
	parametres_calcul_geometrie();

	float boiteX;
	float boiteY;
	float boiteZ;

	float ajustement_debut_x;
	float ajustement_extension_x;
	float ajustement_fin_x;
};

struct parametres_entrainement_texture
{
	parametres_entrainement_texture();

	bool continuer;
	int epochs;
	bool utiliser_gpu;
};

struct resultats_entrainement_texture
{
	resultats_entrainement_texture();

	bool nouveau_tick;
	int epoch;
	float gLoss;
	float dLoss;
	float lossL1;
	float ganLoss;
};














struct projet
{
	callback_log cb_log;

	std::thread thread_entrainement_texture;

	std::string url_dossier;

	std::vector<std::string> noms_scripts;
	std::vector<std::string> noms_categories;
	std::map<std::string, std::vector<std::string>> noms_schemas;
	std::map<std::string, std::vector<std::string>> noms_ias;

	std::map<std::string, sauvegarde_geometrie> sauvegardes_chargees;
	sauvegarde_geometrie sauvegarde_chargee_editeur;


	std::atomic<bool> ecriture_resultats_entrainement_en_cours{ false };
	std::atomic<bool> resultats_entrainement_disponibles{ false };
	std::atomic<bool> devra_arreter_entrainement_au_prochain_tick{ false };
	resultats_entrainement_texture resultats;
};

extern projet projet_actuel;







void sauvegarder_fichier_off(
	const std::string& nom_fichier,
	const geometrie& g);













namespace organisation
{
	void creer_projet(
		const std::string& url_dossier);

	void charger_projet(
		const std::string& url_dossier);
}


namespace preparation
{
	sauvegarde_geometrie calculer_geometrie(
		const geometrie& geometrie,
		const parametres_calcul_geometrie& parametres);

	sauvegarde_geometrie charger_geometrie( // expose pour api, utile pour charger_projet
		const std::string categorie,
		const std::string nom_schema);

	void sauvegarder_geometrie(
		const std::string nom_categorie,
		const std::string nom_fichier);

	void commencer_entrainement_texture(
		const std::string& categorie,
		const std::string& nom_ia,
		const parametres_entrainement_texture& parametres);

	resultats_entrainement_texture obtenir_resultats_entrainement_texture();

	void interrompre_entrainement_texture();
}


namespace generation
{
	geometrie generer_geometrie(
		const sauvegarde_geometrie& s,
		const float taille_x);

	void generer_texture(
		const std::string& nom_categorie,
		const std::string& nom_ia,
		const std::string& url_tex_label,
		const std::string& url_tex_target,
		const bool utiliser_gpu);

	// void executer_script(
	// 	std::string nom_script,
	// 	callback_photographie cb_photo,
	// 	callback_geometrie cb_geo);
}