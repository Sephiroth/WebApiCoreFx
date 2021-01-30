echo ==stop==
docker stop UserCenterApi.dev
echo ==rm==
docker rm UserCenterApi.dev
echo ==rmi==
docker rmi UserCenterApi.dev
echo ==go==
docker-compose up -d --build UserCenterApi.dev