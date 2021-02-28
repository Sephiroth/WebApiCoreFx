echo ==stop==
docker stop usercenterapi.dev
echo ==rm==
docker rm usercenterapi.dev
echo ==rmi==
docker rmi usercenterapi.dev
echo ==go==
docker-compose up -d --build usercenterapi.dev