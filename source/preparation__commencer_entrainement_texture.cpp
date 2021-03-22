#include <generation_procedurale.hpp>
#include "texture_procedurale.h"

void callback_entrainement(int nlhs, mxArray* plhs[], int nrhs, mxArray* prhs[])
{
	log("cb");
	projet_actuel.ecriture_resultats_entrainement_en_cours.store(true);
	projet_actuel.resultats.epoch = (float)mxGetScalar(prhs[0]);
	projet_actuel.resultats.gLoss = (float)mxGetScalar(prhs[1]);
	projet_actuel.resultats.dLoss = (float)mxGetScalar(prhs[2]);
	projet_actuel.resultats.lossL1 = (float)mxGetScalar(prhs[3]);
	projet_actuel.resultats.ganLoss = (float)mxGetScalar(prhs[4]);
	projet_actuel.ecriture_resultats_entrainement_en_cours.store(false);

	nlhs = 1;
	if (projet_actuel.devra_arreter_entrainement_au_prochain_tick.load())
	{
		plhs[0] = mxCreateLogicalScalar(true);
	}
	else
	{
		plhs[0] = mxCreateLogicalScalar(false);
	}



	projet_actuel.resultats.nouveau_tick = true;
	return;
}


void thread_entrainement(
	const std::string& nom_ia,
	const std::string& url_dossier_apprentissage,
	const parametres_entrainement_texture& p)
{
	std::string url_ia = url_dossier_apprentissage + "ias/" + nom_ia;
	std::string url_dossier_tex_label = url_dossier_apprentissage + "textures_label/";
	std::string url_dossier_tex_target = url_dossier_apprentissage + "textures_target/";

	mxArray* ptr_url_ia = mxCreateString(url_ia.c_str());
	mxArray* ptr_url_dossier_tex_label = mxCreateString(url_dossier_tex_label.c_str());
	mxArray* ptr_url_dossier_tex_target = mxCreateString(url_dossier_tex_target.c_str());
	mxArray* ptr_callback = mclCreateSimpleFunctionHandle(callback_entrainement);
	mxArray* ptr_utiliser_gpu = mxCreateLogicalScalar(p.utiliser_gpu);
	mxArray* ptr_continuer = mxCreateLogicalScalar(p.continuer);

	mxArray* ptr[] = {
		ptr_url_ia,
		ptr_url_dossier_tex_label,
		ptr_url_dossier_tex_target,
		ptr_callback,
		ptr_continuer,
		ptr_utiliser_gpu };

	log("avant");
	mlxEntrainer_ia(0, NULL, 6, ptr);
	log("apres");

	return;
}



void preparation::commencer_entrainement_texture(
	const std::string& categorie,
	const std::string& nom_ia,
	const parametres_entrainement_texture& parametres)
{
	demarrer_profilage("commencer_entrainement_texture");
	std::string urlBibliotheque = projet_actuel.url_dossier + "data/" + categorie + "/apprentissage/";

	projet_actuel.resultats = resultats_entrainement_texture();
	projet_actuel.thread_entrainement_texture = std::thread(thread_entrainement, nom_ia, urlBibliotheque, parametres);

	arreter_profilage("commencer_entrainement_texture");
	return;
}