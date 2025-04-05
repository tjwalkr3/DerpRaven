#!/bin/bash
REGISTRY_USER="$1"
REGISTRY_PASS="$2"
IMAGE1="$3"
IMAGE2="$4"
DB_CONN_STRING="$5"
BLOB_CONN_STRING="$6"
BLOB_CONTAINER_NAME="$7"

# Optional: Check if all required arguments are provided
if [ $# -lt 5 ]; then
  echo "Usage: $0 <registry-user> <registry-pass> <image1> <image2> <db-conn-string> <blob-conn-string> <blob-container-name>"
  exit 1
fi

RESOURCE_GROUP="derpraven"
LOCATION="westus3"
CONTAINER_NAME_1="$RESOURCE_GROUP-api"
CONTAINER_NAME_2="$RESOURCE_GROUP-web"

#####################################################
# Create the container service for the API container
#####################################################
(
az container delete \
    --name "$CONTAINER_NAME_1" \
    --resource-group "$RESOURCE_GROUP" \
    --yes

az container create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$CONTAINER_NAME_1" \
    --image "$IMAGE1" \
    --dns-name-label "$CONTAINER_NAME_1" \
    --ports "8080" \
    --os-type Linux \
    --cpu 1 \
    --memory 1.5 \
    --registry-login-server "index.docker.io" \
    --registry-username "$REGISTRY_USER" \
    --registry-password "$REGISTRY_PASS" \
    --location "$LOCATION" \
    --output none \
    --secure-environment-variables \
        ConnectionStrings__DefaultConnection="$DB_CONN_STRING" \
        BlobStorage__ConnectionString="$BLOB_CONN_STRING" \
        BlobStorage__ContainerName="$BLOB_CONTAINER_NAME" \
    || { echo "Failed to create container $CONTAINER_NAME_1"; exit 1; }
) &

#####################################################
# Create the container service for the Web container
#####################################################
(
az container delete \
    --name "$CONTAINER_NAME_2" \
    --resource-group "$RESOURCE_GROUP" \
    --yes

az container create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$CONTAINER_NAME_2" \
    --image $IMAGE2 \
    --dns-name-label "$CONTAINER_NAME_2" \
    --ports "80" \
    --os-type Linux \
    --cpu 1 \
    --memory 1.5 \
    --registry-login-server "index.docker.io" \
    --registry-username "$REGISTRY_USER" \
    --registry-password "$REGISTRY_PASS" \
    --location "$LOCATION" \
    --output none \
    || { echo "Failed to create container $CONTAINER_NAME_1"; exit 1; }
) &

wait
echo "API container created at: http://$CONTAINER_NAME_1.$LOCATION.azurecontainer.io:$PORT"
echo "Web container created at: http://$CONTAINER_NAME_2.$LOCATION.azurecontainer.io:$PORT"
