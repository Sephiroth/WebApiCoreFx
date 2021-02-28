echo ==stop==
docker stop webapicorefx.dev
echo ==rm==
docker rm webapicorefx.dev
echo ==rmi==
docker rmi webapicorefx.dev
echo ==go==
docker-compose up -d --build webapicorefx.dev