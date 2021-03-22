#include <generation_procedurale.hpp>
#include <cmath>

projet projet_actuel;

logique3::logique3()
{
	x = false;
	y = false;
	z = false;
	return;
}
logique3::logique3(bool x, bool y, bool z)
{
	this->x = x;
	this->y = y;
	this->z = z;
	return;
}




reel2::reel2()
{
	x = 0.;
	y = 0.;
	return;
}
reel2::reel2(float x, float y)
{
	this->x = x;
	this->y = y;
	return;
}




reel3::reel3()
{
	x = 0.;
	y = 0.;
	z = 0.;
	return;
}
reel3::reel3(float x, float y, float z)
{
	this->x = x;
	this->y = y;
	this->z = z;
	return;
}
reel3 operator+(const reel3& a, const reel3& b)
{
	return reel3(a.x + b.x, a.y + b.y, a.z + b.z);
}
reel3 operator*(const float x, const reel3& a)
{
	return reel3(x * a.x, x * a.y, x * a.z);
}
reel3 operator*(const reel3& a, const reel3& b)
{
	return reel3(b.x * a.x, b.y * a.y, b.z *a.z);
}
float distance(const reel3& a, const reel3& b)
{
	return std::sqrt(std::pow(a.x - b.x, 2) + std::pow(a.y - b.y, 2) + std::pow(a.z - b.z, 2));
}


entier3::entier3()
{
	x = 0;
	y = 0;
	z = 0;
	return;
}
entier3::entier3(int x, int y, int z)
{
	this->x = x;
	this->y = y;
	this->z = z;
	return;
}
entier3 operator+(const int x, const entier3& a)
{
	return entier3(x + a.x, x + a.y, x + a.z);
}








geometrie::geometrie()
{
	positions.resize(0);
	normales.resize(0);
	uvs.resize(0);
	indices.resize(0);
	return;
}

void transformer_geometrie(
	geometrie& g,
	const reel3 translation,
	const reel3 rotation,
	const reel3 taille)
{
	float facteur_degres_vers_rad = (float)3.141592 / (float)180.;
	auto rotation_rad = facteur_degres_vers_rad * rotation;
	auto sinus = reel3(
		std::sinf(rotation_rad.x),
		std::sinf(rotation_rad.y),
		std::sinf(rotation_rad.z));
	auto cosinus = reel3(
		std::cosf(rotation_rad.x),
		std::cosf(rotation_rad.y),
		std::cosf(rotation_rad.z));
	float Axx = cosinus.x * cosinus.y;
	float Axy = cosinus.x * sinus.y * sinus.z - sinus.x * cosinus.z;
	float Axz = cosinus.x * sinus.y * cosinus.z + sinus.x * sinus.z;
	float Ayx = sinus.x * cosinus.y;
	float Ayy = sinus.x * sinus.y * sinus.z + cosinus.x * cosinus.z;
	float Ayz = sinus.x * sinus.y * cosinus.z - cosinus.x * sinus.z;
	float Azx = -sinus.y;
	float Azy = cosinus.y * sinus.z;
	float Azz = cosinus.y * cosinus.z;

	for (auto iPoint = 0; iPoint < g.positions.size(); iPoint++)
	{
	    reel3 ancienne_position = g.positions[iPoint];
		reel3 ancienne_normale = g.normales[iPoint];

	    // rotation
		auto nouvelle_position = reel3(
			Axx * ancienne_position.x + Axy * ancienne_position.y + Axz * ancienne_position.z,
			Ayx * ancienne_position.x + Ayy * ancienne_position.y + Ayz * ancienne_position.z,
			Azx * ancienne_position.x + Azy * ancienne_position.y + Azz * ancienne_position.z);
		auto nouvelle_normale = reel3(
			Axx * ancienne_normale.x + Axy * ancienne_normale.y + Axz * ancienne_normale.z,
			Ayx * ancienne_normale.x + Ayy * ancienne_normale.y + Ayz * ancienne_normale.z,
			Azx * ancienne_normale.x + Azy * ancienne_normale.y + Azz * ancienne_normale.z);

	    // echelle
		nouvelle_position = nouvelle_position * taille;

	    // position
		nouvelle_position = nouvelle_position + translation;

	    g.positions[iPoint] = nouvelle_position;
	    g.normales[iPoint] = nouvelle_normale;
		
	}

	return;
}

geometrie combiner_geometries(
	const std::vector<geometrie>& v)
{
	auto g = geometrie();
	int premier_vertex = 0;

	for (auto i = 0; i < (int)v.size(); i++)
	{
		auto n_vertices = (int)v[i].positions.size();
		for (auto i_v = 0; i_v < n_vertices; i_v++)
		{
			g.positions.push_back(v[i].positions[i_v]);
			g.normales.push_back(v[i].normales[i_v]);
			g.uvs.push_back(v[i].uvs[i_v]);
		}

		auto n_triangles = (int)v[i].indices.size();
		for (auto i_t = 0; i_t < n_triangles; i_t++)
		{
			g.indices.push_back(premier_vertex + v[i].indices[i_t]);
		}

		premier_vertex += n_vertices;
	}
	return g;
}








sauvegarde_geometrie::sauvegarde_geometrie()
{
	boiteX = 0.;
	boiteY = 0.;
	boiteZ = 0.;
	ajustement_debut_x = 0.;
	ajustement_extension_x = 0.;
	ajustement_fin_x = 0.;
	a_ligne_decoupe_x_b.resize(0);
	b_ligne_decoupe_x_a.resize(0);
	b_ligne_decoupe_x_b.resize(0);
	c_ligne_decoupe_x_a.resize(0);
	c_ligne_decoupe_x_b.resize(0);
	d_ligne_decoupe_x_a.resize(0);
	return;
}





parametres_calcul_geometrie::parametres_calcul_geometrie()
{
	boiteX = 1.; // afaaire!!!!
	boiteY = 1.;
	boiteZ = 1.;
	ajustement_debut_x = 0.;
	ajustement_extension_x = 0.;
	ajustement_fin_x = 0.;
	return;
}

parametres_entrainement_texture::parametres_entrainement_texture()
{
	continuer = true;
	utiliser_gpu = true;
	return;
}

resultats_entrainement_texture::resultats_entrainement_texture()
{
	nouveau_tick = false;
	epoch = 0;
	gLoss = 0.;
	dLoss = 0.;
	lossL1 = 0.;
	ganLoss = 0.;
	return;
}