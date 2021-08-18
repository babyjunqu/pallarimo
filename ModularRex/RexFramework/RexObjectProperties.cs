﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using log4net;
using OpenMetaverse;

namespace ModularRex.RexFramework
{
    public delegate void OnRexObjectPropertiesUpdateDelegate(UUID id, RexObjectProperties prop, bool dbSave);

    public delegate void OnRexObjectPropertiesDataUpdateDelegate(UUID id, string data, bool dbSave);

    public class RexObjectProperties
    {
        private static readonly ILog m_log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Properties

        private UUID parentObjectID = UUID.Zero;
        public UUID ParentObjectID
        {
            get { return parentObjectID; }
            set { parentObjectID = value; }
        }

        private byte m_RexDrawType = 1;
        public byte RexDrawType
        {
            get { return m_RexDrawType; }
            set
            {
                m_RexDrawType = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private bool m_RexIsVisible = true;
        public bool RexIsVisible
        {
            get { return m_RexIsVisible; }
            set
            {
                m_RexIsVisible = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private bool m_RexCastShadows = false;
        public bool RexCastShadows
        {
            get { return m_RexCastShadows; }
            set
            {
                m_RexCastShadows = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private bool m_RexLightCreatesShadows = false;
        public bool RexLightCreatesShadows
        {
            get { return m_RexLightCreatesShadows; }
            set
            {
                m_RexLightCreatesShadows = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private bool m_RexDescriptionTexture = false;
        public bool RexDescriptionTexture
        {
            get { return m_RexDescriptionTexture; }
            set
            {
                m_RexDescriptionTexture = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private bool m_RexScaleToPrim = false;
        public bool RexScaleToPrim
        {
            get { return m_RexScaleToPrim; }
            set
            {
                m_RexScaleToPrim = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private float m_RexDrawDistance = 0;
        public float RexDrawDistance
        {
            get { return m_RexDrawDistance; }
            set
            {
                m_RexDrawDistance = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private float m_RexLOD = 1.0F;
        public float RexLOD
        {
            get { return m_RexLOD; }
            set
            {
                m_RexLOD = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private UUID m_RexMeshUUID = UUID.Zero;
        public UUID RexMeshUUID
        {
            get { return m_RexMeshUUID; }
            set
            {
                m_RexMeshUUID = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private UUID m_RexCollisionMeshUUID = UUID.Zero;
        public UUID RexCollisionMeshUUID
        {
            get { return m_RexCollisionMeshUUID; }
            set
            {
                m_RexCollisionMeshUUID = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private UUID m_RexParticleScriptUUID = UUID.Zero;
        public UUID RexParticleScriptUUID
        {
            get { return m_RexParticleScriptUUID; }
            set
            {
                m_RexParticleScriptUUID = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private UUID m_RexAnimationPackageUUID = UUID.Zero;
        public UUID RexAnimationPackageUUID
        {
            get { return m_RexAnimationPackageUUID; }
            set
            {
                m_RexAnimationPackageUUID = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private string m_RexAnimationName = String.Empty;
        public string RexAnimationName
        {
            get { return m_RexAnimationName; }
            set
            {
                m_RexAnimationName = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private float m_RexAnimationRate = 1.0F;
        public float RexAnimationRate
        {
            get { return m_RexAnimationRate; }
            set
            {
                m_RexAnimationRate = value;
                SchedulePropertiesUpdate(true);
            }
        }


        #region Material Stuff

        public RexMaterialsDictionary RexMaterials = new RexMaterialsDictionary();
        public RexMaterialsDictionary GetRexMaterials()
        {
            return (RexMaterialsDictionary)RexMaterials.Clone();
        }

        /// <summary>
        /// This is only to be used from NHibernate. Use RexMaterials instead in other cases.
        /// </summary>
        public IList<RexMaterialsDictionaryItem> RexMaterialDictionaryItems
        {
            get
            {
                IList<RexMaterialsDictionaryItem> tempRexMaterialDictionaryItems = new List<RexMaterialsDictionaryItem>();
                foreach (KeyValuePair<uint, UUID> entry in RexMaterials)
                {
                    tempRexMaterialDictionaryItems.Add(new RexMaterialsDictionaryItem(entry));
                }
                return tempRexMaterialDictionaryItems;
            }
            set
            {
                //rexMaterialDictionary = new Dictionary<uint, UUID>();
                if (value != null)
                {
                    foreach (RexMaterialsDictionaryItem e in value)
                    {
                        //rexMaterialDictionary.Add(e.Num, e.AssetID);
                        RexMaterials.Add(e.Num, e.AssetID);
                    }
                }
            }
        }

        #endregion

        private string m_RexClassName = String.Empty;
        public string RexClassName
        {
            get { return m_RexClassName; }
            set
            {
                m_RexClassName = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private UUID m_RexSoundUUID = UUID.Zero;
        public UUID RexSoundUUID
        {
            get { return m_RexSoundUUID; }
            set
            {
                m_RexSoundUUID = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private float m_RexSoundVolume = 0;
        public float RexSoundVolume
        {
            get { return m_RexSoundVolume; }
            set
            {
                m_RexSoundVolume = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private float m_RexSoundRadius = 0;
        public float RexSoundRadius
        {
            get { return m_RexSoundRadius; }
            set
            {
                m_RexSoundRadius = value;
                SchedulePropertiesUpdate(true);
            }
        }

        private string m_rexData = String.Empty;
        public string RexData
        {
            get { return m_rexData; }
            set
            {
                if (value.Length > 3000)
                    m_rexData = value.Substring(0, 3000);
                else
                    m_rexData = value;

                ScheduleDataUpdate(true);
            }
        }

        private int m_RexSelectPriority = 0;
        public int RexSelectPriority
        {
            get { return m_RexSelectPriority; }
            set
            {
                m_RexSelectPriority = value;
                SchedulePropertiesUpdate(true);
            }
        }
        #endregion

        #region Events

        public event OnRexObjectPropertiesDataUpdateDelegate OnDataUpdate;
        public event OnRexObjectPropertiesUpdateDelegate OnPropertiesUpdate;

        #endregion

        /// <summary>
        /// Initialises a new RexObjectProperties class from
        /// the specified binary array. Unpacks the array
        /// according to the viewer-specified format into
        /// the properties.
        /// </summary>
        /// <param name="data"></param>
        public RexObjectProperties(byte[] data)
        {
            SetRexPrimDataFromBytes(data);
        }

        public RexObjectProperties() { }

        #region Old RexServer ToByte/FromByte methods
        public byte[] GetRexPrimDataToBytes()
        {
            try
            {
                // Display
                int size = sizeof(byte) + // drawtype
                    sizeof(bool) + sizeof(bool) + sizeof(bool) + sizeof(bool) + sizeof(bool) + // visible,castshadows,lightcreatesshadows,desctex,scaletoprim
                    sizeof(float) + sizeof(float) + // drawdist,lod
                    16 + 16 + 16 + 16 +  // meshuuid,colmeshuuid,particleuuid,animpackuuid
                    sizeof(int); // selectpriority

                // Animname,animrate
                size += (m_RexAnimationName.Length + 1 + sizeof(float));

                // Materialdata
                size += sizeof(byte); // Number of materials
                RexMaterialsDictionary materials = GetRexMaterials();
                size += (materials.Values.Count * (sizeof(byte) + 16 + sizeof(byte))); // materialassettype,matuuid,matindex

                // Misc
                size = size + m_RexClassName.Length + 1 + // classname & endbyte
                    16 + sizeof(float) + sizeof(float); // sounduuid,sndvolume,sndradius 

                // Build byte array
                byte[] buffer = new byte[size];
                int idx = 0;

                buffer[idx++] = m_RexDrawType;
                BitConverter.GetBytes(m_RexIsVisible).CopyTo(buffer, idx);
                idx += sizeof(bool);
                BitConverter.GetBytes(m_RexCastShadows).CopyTo(buffer, idx);
                idx += sizeof(bool);
                BitConverter.GetBytes(m_RexLightCreatesShadows).CopyTo(buffer, idx);
                idx += sizeof(bool);
                BitConverter.GetBytes(m_RexDescriptionTexture).CopyTo(buffer, idx);
                idx += sizeof(bool);
                BitConverter.GetBytes(m_RexScaleToPrim).CopyTo(buffer, idx);
                idx += sizeof(bool);

                BitConverter.GetBytes(m_RexDrawDistance).CopyTo(buffer, idx);
                idx += sizeof(float);
                BitConverter.GetBytes(m_RexLOD).CopyTo(buffer, idx);
                idx += sizeof(float);

                m_RexMeshUUID.GetBytes().CopyTo(buffer, idx);
                idx += 16;
                m_RexCollisionMeshUUID.GetBytes().CopyTo(buffer, idx);
                idx += 16;
                m_RexParticleScriptUUID.GetBytes().CopyTo(buffer, idx);
                idx += 16;

                m_RexAnimationPackageUUID.GetBytes().CopyTo(buffer, idx);
                idx += 16;
                Encoding.ASCII.GetBytes(m_RexAnimationName).CopyTo(buffer, idx);
                idx += (m_RexAnimationName.Length);
                buffer[idx++] = 0;
                BitConverter.GetBytes(m_RexAnimationRate).CopyTo(buffer, idx);
                idx += sizeof(float);

                buffer[idx++] = (byte)materials.Values.Count;
                foreach (KeyValuePair<uint, UUID> kvp in materials)
                {
                    // Removed - do we really need to know this?
                    // Adds a dependency for a single scene method
                    // which may break in future. Dont see why
                    // the client needs it either.

                    /*      
                    AssetBase tempmodel = m_parentGroup.Scene.AssetCache.FetchAsset(kvp.Value); // materialassettype
                    if (tempmodel != null)
                    {
                        byte temptype = (byte)(tempmodel.Type);
                        buffer[idx++] = temptype;
                    }
                    else
                    */
                    buffer[idx++] = 0;

                    kvp.Value.GetBytes().CopyTo(buffer, idx); // matuuid 
                    idx += 16;

                    byte tempindex = (byte)kvp.Key; // matindex
                    buffer[idx++] = tempindex;
                }

                Encoding.ASCII.GetBytes(m_RexClassName).CopyTo(buffer, idx);
                idx += (m_RexClassName.Length);
                buffer[idx++] = 0;

                m_RexSoundUUID.GetBytes().CopyTo(buffer, idx);
                idx += 16;
                BitConverter.GetBytes(m_RexSoundVolume).CopyTo(buffer, idx);
                idx += sizeof(float);
                BitConverter.GetBytes(m_RexSoundRadius).CopyTo(buffer, idx);
                idx += sizeof(float);

                BitConverter.GetBytes(m_RexSelectPriority).CopyTo(buffer, idx);
// ReSharper disable RedundantAssignment
                idx += sizeof(int);
// ReSharper restore RedundantAssignment

                return buffer;
            }
            catch (Exception e)
            {
                m_log.Error(e.ToString());
                return null;
            }
        }

        public void SetRexPrimDataFromBytes(byte[] bytes)
        {
            try
            {
                int idx = 0;
                RexDrawType = bytes[idx++];

                RexIsVisible = BitConverter.ToBoolean(bytes, idx);
                idx += sizeof(bool);
                RexCastShadows = BitConverter.ToBoolean(bytes, idx);
                idx += sizeof(bool);
                RexLightCreatesShadows = BitConverter.ToBoolean(bytes, idx);
                idx += sizeof(bool);
                RexDescriptionTexture = BitConverter.ToBoolean(bytes, idx);
                idx += sizeof(bool);
                RexScaleToPrim = BitConverter.ToBoolean(bytes, idx);
                idx += sizeof(bool);

                RexDrawDistance = BitConverter.ToSingle(bytes, idx);
                idx += sizeof(float);
                RexLOD = BitConverter.ToSingle(bytes, idx);
                idx += sizeof(float);

                RexMeshUUID = new UUID(bytes, idx);
                idx += 16;
                RexCollisionMeshUUID = new UUID(bytes, idx);
                idx += 16;
                RexParticleScriptUUID = new UUID(bytes, idx);
                idx += 16;

                // animation
                RexAnimationPackageUUID = new UUID(bytes, idx);
                idx += 16;
                StringBuilder bufferanimname = new StringBuilder();
                while ((idx < bytes.Length) && (bytes[idx] != 0))
                {
                    char c = (char)bytes[idx++];
                    bufferanimname.Append(c);
                }
                RexAnimationName = bufferanimname.ToString();
                idx++;
                RexAnimationRate = BitConverter.ToSingle(bytes, idx);
                idx += sizeof(float);

                // materials, before setting materials clear them
                RexMaterials.ClearMaterials();
                byte matcount = bytes[idx++];
                for (int i = 0; i < matcount; i++)
                {
                    idx++; // skip type
                    UUID matuuid = new UUID(bytes, idx);
                    idx += 16;
                    byte matindex = bytes[idx++];
                    RexMaterials.AddMaterial(Convert.ToUInt32(matindex), matuuid);
                }

                // misc
                StringBuilder buffer = new StringBuilder();
                while ((idx < bytes.Length) && (bytes[idx] != 0))
                {
                    char c = (char)bytes[idx++];
                    buffer.Append(c);
                }
                RexClassName = buffer.ToString();
                idx++;

                RexSoundUUID = new UUID(bytes, idx);
                idx += 16;
                RexSoundVolume = BitConverter.ToSingle(bytes, idx);
                idx += sizeof(float);
                RexSoundRadius = BitConverter.ToSingle(bytes, idx);
                idx += sizeof(float);

                if (bytes.Length >= (idx + sizeof(int)))
                {
                    RexSelectPriority = BitConverter.ToInt32(bytes, idx);
// ReSharper disable RedundantAssignment
                    idx += sizeof(int);
// ReSharper restore RedundantAssignment
                }

                SchedulePropertiesUpdate(true);
            }
            catch (Exception e)
            {
                m_log.Error(e.ToString());
            }
        }
        #endregion

        #region Debug
        public void PrintRexPrimdata()
        {
            try
            {
                m_log.Warn("RexDrawType:" + RexDrawType);
                m_log.Warn("RexIsVisible:" + RexIsVisible);
                m_log.Warn("RexCastShadows:" + RexCastShadows);
                m_log.Warn("RexLightCreatesShadows:" + RexLightCreatesShadows);
                m_log.Warn("RexDescriptionTexture:" + RexDescriptionTexture);
                m_log.Warn("RexScaleToPrim:" + RexScaleToPrim);
                m_log.Warn("RexDrawDistance:" + RexDrawDistance);
                m_log.Warn("RexLOD" + RexLOD);
                m_log.Warn("RexMeshUUID:" + RexMeshUUID);
                m_log.Warn("RexCollisionMeshUUID:" + RexCollisionMeshUUID);
                m_log.Warn("RexParticleScriptUUID:" + RexParticleScriptUUID);
                m_log.Warn("RexAnimationPackageUUID:" + RexAnimationPackageUUID);
                m_log.Warn("RexAnimationName:" + RexAnimationName);
                m_log.Warn("RexAnimationRate:" + RexAnimationRate);
                m_log.Warn("RexMaterials:" + RexMaterials);
                m_log.Warn("RexClassName:" + RexClassName);
                m_log.Warn("RexSoundUUID:" + RexSoundUUID);
                m_log.Warn("RexSoundVolume:" + RexSoundVolume);
                m_log.Warn("RexSoundRadius:" + RexSoundRadius);
                m_log.Warn("RexSelectPriority:" + RexSelectPriority);
            }
            catch (Exception e)
            {
                m_log.Error(e.ToString());
            }
        }
        #endregion

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="saveToDB"></param>
        public void SchedulePropertiesUpdate(bool saveToDB)
        {
            if(OnPropertiesUpdate != null)
            {
                OnPropertiesUpdate(ParentObjectID, this, saveToDB);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="saveToDB"></param>
        public void ScheduleDataUpdate(bool saveToDB)
        {
            if(OnDataUpdate != null)
            {
                OnDataUpdate(ParentObjectID, RexData, saveToDB);
            }
        }


    }
}
