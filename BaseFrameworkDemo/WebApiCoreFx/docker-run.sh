echo ==stop==
docker stop WebApiCoreFx.dev
echo ==rm==
docker rm WebApiCoreFx.dev
echo ==rmi==
docker rmi WebApiCoreFx.dev
echo ==go==
docker-compose up -d --build WebApiCoreFx.dev