pack build pradeepl/k8shealth:0.1 --builder heroku/buildpacks:20

 

# Package the application into a container image
pack build k8shealth:0.1 --builder paketobuildpacks/builder:base

docker build -t k8shealth:0.7  -f Dockerfile .   

kind load docker-image k8shealth:0.7

kubectl delete -f deployment.yaml
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml

kubectl port-forward svc/dotnet-webapi-service 8080:80
