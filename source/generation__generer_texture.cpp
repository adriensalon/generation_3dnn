#include "GPROC.h"


#include <textures_ia.h>

void generation::generer_texture(
	const std::string& nom_categorie,
	const std::string& nom_ia,
	const std::string& url_tex_label,
	const std::string& url_tex_target,
	const bool utiliser_gpu)
{
	demarrer_profilage("generer_texture");
	
	std::string url_ia = projet_actuel.url_dossier + "/data/" + nom_categorie + "/apprentissage/ias/" + nom_ia + ".mat";

	mxArray* ptr_url_ia = mxCreateString(url_ia.c_str());
	mxArray* ptr_url_predict_label = mxCreateString(url_tex_label.c_str());
	mxArray* ptr_url_predict_target = mxCreateString(url_tex_target.c_str());
	mxArray* ptr_utiliser_gpu = mxCreateLogicalScalar(utiliser_gpu);

	mlfPredire_ia(
		ptr_url_ia,
		ptr_url_predict_label,
		ptr_url_predict_target,
		ptr_utiliser_gpu);

	arreter_profilage("generer_texture");
	return;
}