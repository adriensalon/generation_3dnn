#include <generation_procedurale.hpp>

#include <cmath>


void sauvegarder_fichier_off(
	const std::string& nom_fichier,
	const geometrie& g)
{
	std::ofstream stream_fichier(nom_fichier);
	if (stream_fichier.good())
	{
		auto nP = (int)g.positions.size();
		auto nT = (int)g.indices.size();
		stream_fichier << "OFF" << "\n";
		stream_fichier << nP << " " << nT << " 0" << "\n";
		for (auto iVertex = 0; iVertex < nP; iVertex++)
		{
			stream_fichier << g.positions[iVertex].x << " " << g.positions[iVertex].y << " " << g.positions[iVertex].z << "\n";
		}
		for (auto iTriangle = 0; iTriangle < nT; iTriangle++)
		{
			stream_fichier << "3  " << g.indices[iTriangle].x << " " << g.indices[iTriangle].y << " " << g.indices[iTriangle].z << "\n";
		}
		stream_fichier.close();
	}
	return;
}

geometrie charger_fichier_off(
	const std::string& nom_fichier)
{
	std::ifstream stream_fichier(nom_fichier);
	std::string ligne_fichier;
	std::getline(stream_fichier, ligne_fichier); // OFF

	std::getline(stream_fichier, ligne_fichier);
	auto premier_blanc = ligne_fichier.find_first_of(" ");
	auto nombre_vertices = std::stoi(ligne_fichier.substr(0, premier_blanc));
	ligne_fichier = ligne_fichier.substr(premier_blanc + (size_t)1);
	premier_blanc = ligne_fichier.find_first_of(" ");
	int nombre_triangles = std::stoi(ligne_fichier.substr(0, premier_blanc));


	auto g = geometrie();
	g.positions.resize(nombre_vertices);
	g.normales = std::vector<reel3>(nombre_vertices, reel3(0., 0., 0.));
	g.indices.resize(nombre_triangles);

	for (auto iVertex = 0; iVertex < nombre_vertices; iVertex++)
	{
		std::getline(stream_fichier, ligne_fichier);
		auto premierBlanc = ligne_fichier.find_first_of(" ");
		g.positions[iVertex].x = std::stof(ligne_fichier.substr(0, premierBlanc));
		ligne_fichier = ligne_fichier.substr(premierBlanc + (size_t)1);
		premierBlanc = ligne_fichier.find_first_of(" ");
		g.positions[iVertex].y = std::stof(ligne_fichier.substr(0, premierBlanc));
		g.positions[iVertex].z = std::stof(ligne_fichier.substr(premierBlanc + 1));
	}
	for (auto iTriangle = 0; iTriangle < nombre_triangles; iTriangle++)
	{
		std::getline(stream_fichier, ligne_fichier);
		int min = ligne_fichier.c_str()[2] == ' ' ? 3 : 2;
		ligne_fichier = ligne_fichier.substr(min);
		auto premierBlanc = ligne_fichier.find_first_of(" ");
		g.indices[iTriangle].x = std::stoi(ligne_fichier.substr(0, premierBlanc));
		ligne_fichier = ligne_fichier.substr(premierBlanc + (size_t)1);
		premierBlanc = ligne_fichier.find_first_of(" ");
		g.indices[iTriangle].y = std::stoi(ligne_fichier.substr(0, premierBlanc));
		g.indices[iTriangle].z = std::stoi(ligne_fichier.substr(premierBlanc + 1));
	}
	stream_fichier.close();
	return g;
}







std::vector<int> decouper_geometrie_x(
	geometrie& g, 
	float decoupe_x, 
	bool garder_avant_ou_apres)
{
	auto out_g = geometrie();
	auto out_v = std::vector<int>();
	int nombre_vertices = 0;

	for (auto i = 0; i < g.indices.size(); i++)
	{
		int indice_a = g.indices[i].x;
		int indice_b = g.indices[i].y;
		int indice_c = g.indices[i].z;
		reel3 p_a = g.positions[indice_a];
		reel3 p_b = g.positions[indice_b];
		reel3 p_c = g.positions[indice_c];

		bool a_dedans;
		bool b_dedans;
		bool c_dedans;
		if (!garder_avant_ou_apres)
		{
			a_dedans = p_a.x < decoupe_x;
			b_dedans = p_b.x < decoupe_x;
			c_dedans = p_c.x < decoupe_x;
		}
		else
		{
			a_dedans = p_a.x > decoupe_x;
			b_dedans = p_b.x > decoupe_x;
			c_dedans = p_c.x > decoupe_x;
		}
		int nombre_vertices_dedans = (int)a_dedans + (int)b_dedans + (int)c_dedans;

		switch (nombre_vertices_dedans)
		{
		case (3):
		{
			// ajouter le triangle direct;
			out_g.positions.push_back(p_a);
			out_g.positions.push_back(p_b);
			out_g.positions.push_back(p_c);

			reel3 n_a = g.normales[indice_a];
			reel3 n_b = g.normales[indice_b];
			reel3 n_c = g.normales[indice_c];
			reel2 uv_a = g.uvs[indice_a];
			reel2 uv_b = g.uvs[indice_b];
			reel2 uv_c = g.uvs[indice_c];
			out_g.normales.push_back(n_a);
			out_g.normales.push_back(n_b);
			out_g.normales.push_back(n_c);
			out_g.uvs.push_back(uv_a);
			out_g.uvs.push_back(uv_b);
			out_g.uvs.push_back(uv_c);
			out_g.indices.push_back(entier3(
				nombre_vertices,
				nombre_vertices + 1,
				nombre_vertices + 2));
			nombre_vertices += 3;
			break;
			break;
		}
		case (2):
		{
			//		f	i
			//		|	\
				//		|		h
				//		|	/	
				//		g	j

				// h, f, g
			int indice_h;
			int indice_f;
			int indice_g;
			if (!a_dedans)
			{
				indice_h = indice_a;
				indice_f = indice_b;
				indice_g = indice_c;
			}
			else if (!b_dedans)
			{
				indice_h = indice_b;
				indice_f = indice_c;
				indice_g = indice_a;
			}
			else if (!c_dedans)
			{
				indice_h = indice_c;
				indice_f = indice_a;
				indice_g = indice_b;
			}

			// i
			auto p_h = g.positions[indice_h];
			auto n_h = g.normales[indice_h];
			auto uv_h = g.uvs[indice_h];

			auto p_f = g.positions[indice_f];
			auto n_f = g.normales[indice_f];
			auto uv_f = g.uvs[indice_f];
			float r_f = std::abs(p_f.x - decoupe_x) / distance(p_f, p_h);
			auto p_i = reel3(
				decoupe_x,
				std::lerp(p_f.y, p_h.y, r_f),
				std::lerp(p_f.z, p_h.z, r_f));
			auto n_i = reel3(
				std::lerp(n_f.x, n_h.x, r_f),
				std::lerp(n_f.y, n_h.y, r_f),
				std::lerp(n_f.z, n_h.z, r_f));
			auto uv_i = reel2(
				std::lerp(uv_f.x, uv_h.x, r_f),
				std::lerp(uv_f.y, uv_h.y, r_f));

			// j
			reel3 p_g = g.positions[indice_g];
			reel3 n_g = g.normales[indice_g];
			auto uv_g = g.uvs[indice_g];
			float r_g = std::abs(p_g.x - decoupe_x) / distance(p_g, p_h);
			reel3 p_j = reel3(
				decoupe_x,
				std::lerp(p_g.y, p_h.y, r_g),
				std::lerp(p_g.z, p_h.z, r_g));
			reel3 n_j = reel3(
				std::lerp(n_g.x, n_h.x, r_g),
				std::lerp(n_g.y, n_h.y, r_g),
				std::lerp(n_g.z, n_h.z, r_g));
			reel2 uv_j = reel2(
				std::lerp(uv_g.x, uv_h.x, r_g),
				std::lerp(uv_g.y, uv_h.y, r_g));

			out_g.positions.push_back(p_f);
			out_g.positions.push_back(p_g);
			out_g.positions.push_back(p_j);
			out_g.positions.push_back(p_i);
			out_g.normales.push_back(n_f);
			out_g.normales.push_back(n_g);
			out_g.normales.push_back(n_j);
			out_g.normales.push_back(n_i);
			out_g.uvs.push_back(uv_f);
			out_g.uvs.push_back(uv_g);
			out_g.uvs.push_back(uv_j);
			out_g.uvs.push_back(uv_i);



			// triangles
			auto nouvel_indice_f = nombre_vertices;
			auto nouvel_indice_g = nombre_vertices + 1;
			auto nouvel_indice_j = nombre_vertices + 2;
			auto nouvel_indice_i = nombre_vertices + 3;
			out_v.push_back(nouvel_indice_i);
			out_v.push_back(nouvel_indice_j);
			out_g.indices.push_back(entier3(nouvel_indice_f, nouvel_indice_g, nouvel_indice_j));
			out_g.indices.push_back(entier3(nouvel_indice_j, nouvel_indice_i, nouvel_indice_f));
			nombre_vertices += 4;
			break;
		}
		case (1):
		{
			//		p	i
			//		|	\
				//		|		t
				//		|	/	
				//		q	j

				// t, p, q
			int indice_t;
			int indice_p;
			int indice_q;
			if (a_dedans)
			{
				indice_t = indice_a;
				indice_p = indice_b;
				indice_q = indice_c;
			}
			else if (b_dedans)
			{
				indice_t = indice_b;
				indice_p = indice_c;
				indice_q = indice_a;
			}
			else if (c_dedans)
			{
				indice_t = indice_c;
				indice_p = indice_a;
				indice_q = indice_b;
			}

			// t
			auto p_t = g.positions[indice_t];
			auto n_t = g.normales[indice_t];
			auto uv_t = g.uvs[indice_t];

			// i
			auto p_p = g.positions[indice_p];
			auto n_p = g.normales[indice_p];
			auto uv_p = g.uvs[indice_p];
			float r_p = std::abs(p_t.x - decoupe_x) / distance(p_p, p_t);
			auto p_i = reel3(
				decoupe_x,
				std::lerp(p_t.y, p_p.y, r_p),
				std::lerp(p_t.z, p_p.z, r_p));
			auto n_i = reel3(
				std::lerp(n_t.x, n_p.x, r_p),
				std::lerp(n_t.y, n_p.y, r_p),
				std::lerp(n_t.z, n_p.z, r_p));
			auto uv_i = reel2(
				std::lerp(uv_t.x, uv_p.x, r_p),
				std::lerp(uv_t.y, uv_p.y, r_p));

			// j
			reel3 p_q = g.positions[indice_q];
			reel3 n_q = g.normales[indice_q];
			auto uv_q = g.uvs[indice_q];
			float r_q = std::abs(p_t.x - decoupe_x) / distance(p_q, p_t);
			reel3 p_j = reel3(
				decoupe_x,
				std::lerp(p_t.y, p_q.y, r_q),
				std::lerp(p_t.z, p_q.z, r_q));
			reel3 n_j = reel3(
				std::lerp(n_t.x, n_q.x, r_q),
				std::lerp(n_t.y, n_q.y, r_q),
				std::lerp(n_t.z, n_q.z, r_q));
			reel2 uv_j = reel2(
				std::lerp(uv_t.x, uv_q.x, r_q),
				std::lerp(uv_t.y, uv_q.y, r_q));

			out_g.positions.push_back(p_t);
			out_g.positions.push_back(p_j);
			out_g.positions.push_back(p_i);
			out_g.normales.push_back(n_t);
			out_g.normales.push_back(n_j);
			out_g.normales.push_back(n_i);
			out_g.uvs.push_back(uv_t);
			out_g.uvs.push_back(uv_j);
			out_g.uvs.push_back(uv_i);

			// triangle
			out_v.push_back(nombre_vertices + 1);
			out_v.push_back(nombre_vertices + 2);
			out_g.indices.push_back(entier3(
				nombre_vertices,
				nombre_vertices + 1,
				nombre_vertices + 2));
			nombre_vertices += 3;
			break;
		}
		}
	}










	g = out_g;
	return out_v;
}

void aligner_bords_x(
	const geometrie& g1,
	const geometrie& g2,
	std::vector<int>& indices1,
	std::vector<int>& indices2)
{
	auto nombreIndices = indices1.size();
	auto indicesSwap1 = std::vector<int>(nombreIndices);

	for (int i = 0; i < nombreIndices; i++) // aligner b en face de c
	{
		for (int j = 0; j < nombreIndices; j++)
		{
			if (g1.positions[indices1[i]].y == g2.positions[indices2[j]].y && g1.positions[indices1[i]].z == g2.positions[indices2[j]].z)
			{
				indicesSwap1[j] = indices1[i];
				break;
			}
		}
	}

	auto indicesAlignes1 = std::vector<int>(nombreIndices);
	auto indicesAlignes2 = std::vector<int>(nombreIndices);
	indicesAlignes1[0] = indicesSwap1[0];
	indicesAlignes2[0] = indices2[0];
	for (int i = 1; i < nombreIndices; i++)// reorganiser b et c pour que les vertices se suivent
	{
		float precPosY = g1.positions[indicesAlignes1[i - 1]].y;
		float precPosZ = g1.positions[indicesAlignes1[i - 1]].z;

		auto indiceDistanceMin = -1;
		auto distanceMin = 1000.;
		for (int j = 0; j < nombreIndices; j++)
		{
			auto d = std::sqrtf(std::powf(g1.positions[indicesSwap1[j]].y - precPosY, 2) + std::powf(g1.positions[indicesSwap1[j]].z - precPosZ, 2));
			if (d > 0. && d < distanceMin)
			{
				bool dejaTrouve = false;
				for (int k = 0; k < i; k++)
				{
					if (g1.positions[indicesAlignes1[k]].y == g1.positions[indicesSwap1[j]].y && g1.positions[indicesAlignes1[k]].z == g1.positions[indicesSwap1[j]].z)
					{
						dejaTrouve = true;
						break;
					}
				}
				if (!dejaTrouve)
				{
					distanceMin = d;
					indiceDistanceMin = j;
				}
			}
		}
		indicesAlignes1[i] = indicesSwap1[indiceDistanceMin];
		indicesAlignes2[i] = indices2[indiceDistanceMin];
	}
	indices1 = indicesAlignes1;
	indices2 = indicesAlignes2;
	return;
}

sauvegarde_geometrie preparation::calculer_geometrie(
	const geometrie& g,
	const parametres_calcul_geometrie& parametres)
{
	demarrer_profilage("calculer_geometrie");

	auto s = sauvegarde_geometrie();
	s.ajustement_debut_x = parametres.ajustement_debut_x;
	s.ajustement_extension_x = parametres.ajustement_extension_x;
	s.ajustement_fin_x = parametres.ajustement_fin_x;
	s.boiteX = parametres.boiteX;
	s.boiteY = parametres.boiteY;
	s.boiteZ = parametres.boiteZ;

	s.a = geometrie();
	s.d = geometrie();
	auto g_bc = geometrie();
	s.a = g;
	s.d = g;
	g_bc = g;
	s.a_ligne_decoupe_x_b = decouper_geometrie_x(s.a, parametres.ajustement_debut_x, false);
	s.b_ligne_decoupe_x_a = decouper_geometrie_x(g_bc, parametres.ajustement_debut_x, true);
	s.c_ligne_decoupe_x_b = decouper_geometrie_x(g_bc, parametres.ajustement_fin_x, false);
	s.d_ligne_decoupe_x_a = decouper_geometrie_x(s.d, parametres.ajustement_fin_x, true);
	s.b = geometrie();
	s.c = geometrie();
	s.b = g_bc;
	s.c = g_bc;
	s.b_ligne_decoupe_x_b = decouper_geometrie_x(s.b, parametres.ajustement_extension_x, false);
	s.c_ligne_decoupe_x_a = decouper_geometrie_x(s.c, parametres.ajustement_extension_x, true);

	sauvegarder_fichier_off("C:/Users/adri/Desktop/g_a.off", s.a);
	sauvegarder_fichier_off("C:/Users/adri/Desktop/g_b.off", s.b);
	sauvegarder_fichier_off("C:/Users/adri/Desktop/g_c.off", s.c);
	sauvegarder_fichier_off("C:/Users/adri/Desktop/g_d.off", s.d);


	//aligner_bords_x(s.a, s.b, s.a_ligne_decoupe_x_b, s.b_ligne_decoupe_x_a);
	//aligner_bords_x(s.b, s.c, s.b_ligne_decoupe_x_b, s.c_ligne_decoupe_x_a);
	//aligner_bords_x(s.c, s.d, s.c_ligne_decoupe_x_b, s.d_ligne_decoupe_x_a);

	arreter_profilage("calculer_geometrie");
	return s;
}