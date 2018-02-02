/* Copyright (c) 2012 MoPho' Games
 * All Rights Reserved
 * 
 * Please see the included 'LICENSE.TXT' for usage rights
 * If this asset was downloaded from the Unity Asset Store,
 * you may instead refer to the Unity Asset Store Customer EULA
 * If the asset was NOT purchased or downloaded from the Unity
 * Asset Store and no such 'LICENSE.TXT' is present, you may
 * assume that the software has been pirated.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using UnityEngine;

namespace MoPhoGames.USpeak.Codec
{
    public class SpeexCodec : ICodec
	{
		private NSpeex.SpeexDecoder m_wide_dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.Wide );
		private NSpeex.SpeexEncoder m_wide_enc = new NSpeex.SpeexEncoder( NSpeex.BandMode.Wide );

		private NSpeex.SpeexDecoder m_narrow_dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.Narrow );
		private NSpeex.SpeexEncoder m_narrow_enc = new NSpeex.SpeexEncoder( NSpeex.BandMode.Narrow );

		#region Private Methods

		private int next_multiple( int num, int multiple )
		{
			int t = multiple + num;
			return ( t - ( t % multiple ) );
		}

		private byte[] encode( short[] rawData, BandMode mode )
		{
			//var encoder = new NSpeex.SpeexEncoder( NSpeex.BandMode.Narrow );

			rawData = ExpandShortArray( rawData );

			if( mode == BandMode.Wide )
				rawData = PadShortArray( rawData, 223 );
			else
				rawData = PadShortArray( rawData, 80 );

			var encoder = m_wide_enc;
			if( mode == BandMode.Narrow )
				encoder = m_narrow_enc;

			encoder.Quality = 1;
			encoder.VBR = true;

			var inDataSize = rawData.Length;
			//var padding = ( inDataSize % encoder.FrameSize );
			int mult_factor = encoder.FrameSize;
			int half_mult_factor = mult_factor / 2;

			//var last_data_size = inDataSize;
			//var indatasize2 = next_multiple( inDataSize, mult_factor );
			//Debug.Log( inDataSize );
			inDataSize = mult_factor * ( ( inDataSize + half_mult_factor ) / mult_factor ); //find the next multiple of frame size
			//inDataSize = indatasize2;

			//if( inDataSize < last_data_size )
			//{
			//    Debug.LogWarning( last_data_size + ", " + inDataSize );
			//}

			while( inDataSize < rawData.Length )
			{
				inDataSize += encoder.FrameSize * 2;
			}

			int padding = inDataSize - rawData.Length;

			var inData = new short[ inDataSize ];
			System.Array.Copy( rawData, 0, inData, 0, rawData.Length );

			// inData = ExpandShortArray( inData );

			//var encodingFrameSize = encoder.FrameSize;
			var encodedBuffer = new byte[ rawData.Length * 2 ];
			try
			{
				var byte_count = encoder.Encode( inData, 0, inData.Length, encodedBuffer, 0, encodedBuffer.Length );
				var encd = new byte[ byte_count + 8 ];
				System.Array.Copy( encodedBuffer, 0, encd, 8, byte_count );
				byte[] sample_count = BitConverter.GetBytes( rawData.Length );
				byte[] pad_bc = BitConverter.GetBytes( padding );
				System.Array.Copy( sample_count, 0, encd, 0, 4 );
				System.Array.Copy( pad_bc, 0, encd, 4, 4 );
				return encd;
			}
			catch( Exception e )
			{
				Debug.Log( e.Message );
				throw e;
			}
		}

		private short[] decode( byte[] rawData, BandMode mode )
		{
			//NSpeex.SpeexDecoder dec = new NSpeex.SpeexDecoder( NSpeex.BandMode.Narrow );
			var dec = m_wide_dec;
			if( mode == BandMode.Narrow )
				dec = m_narrow_dec;

			//number of samples
			int sampleCount = BitConverter.ToInt32( rawData, 0 );
			//int padding = BitConverter.ToInt32( rawData, 4 );

			short[] decoded_samples = new short[ sampleCount * 2 ];

			byte[] in_data = new byte[ rawData.Length - 8 ];
			System.Array.Copy( rawData, 8, in_data, 0, in_data.Length );

			int decoded_count = dec.Decode( in_data, 0, in_data.Length, decoded_samples, 0, false );
			if( decoded_count != 0 )
			{
				System.Array.Resize<short>( ref decoded_samples, decoded_count );

				//trim lookahead values
				int lookahead = 446;
				if( mode == BandMode.Narrow )
					lookahead = 160;

				short[] ret = new short[ decoded_samples.Length - lookahead ];
				System.Array.Copy( decoded_samples, lookahead, ret, 0, ret.Length );

				short[] cont = ContractShortArray( ret );

				return cont;
			}
			else
			{
				return new short[] { 0 };
			}
		}

		private short[] PadShortArray( short[] src_array, int pad )
		{
			short[] expanded = new short[ src_array.Length + pad ];
			System.Array.Copy( src_array, 0, expanded, pad, src_array.Length );
			return expanded;
		}

		private short[] ExpandShortArray( short[] src_array )
		{
			short[] expanded = new short[ src_array.Length * 2 ];
			for( int i = 0; i < src_array.Length; i++ )
			{
				expanded[ i * 2 ] = src_array[ i ];
				expanded[ ( i * 2 ) + 1 ] = src_array[ i ];
			}
			return expanded;
		}

		private short[] ContractShortArray( short[] src_array )
		{
			short[] contracted = new short[ src_array.Length / 2 ];
			for( int i = 0; i < contracted.Length; i++ )
			{
				contracted[ i ] = src_array[ i * 2 ];
			}
			return contracted;
		}

		private byte[] EncodeAudio( short[] rawData )
		{
			var encoder = new NSpeex.SpeexEncoder( NSpeex.BandMode.Wide );
			encoder.Quality = 1;
			var encodedData = new List<byte[]>();

			var inDataSize = rawData.Length;
			inDataSize = inDataSize + inDataSize % encoder.FrameSize;
			var inData = new short[ inDataSize ];
			System.Array.Copy( rawData, inData, rawData.Length );

			var encodingFrameSize = encoder.FrameSize;
			var encodedBuffer = new byte[ 1024 ];
			for( var offset = 0; offset + encodingFrameSize < inDataSize; offset += encodingFrameSize )
			{
				var encodedBytes = encoder.Encode( inData, offset, encodingFrameSize, encodedBuffer, 0, encodingFrameSize );
				var encodedFrame = new byte[ encodedBytes ];
				System.Array.Copy( encodedBuffer, 0, encodedFrame, 0, encodedBytes );
				encodedData.Add( encodedFrame );
			}

			var ms = new MemoryStream();

			var countBits = BitConverter.GetBytes( (Int32)encodedData.Count );
			ms.Write( countBits, 0, countBits.Length );

			for( int i = 0; i < encodedData.Count; i++ )
			{
				var d = encodedData[ i ];
				var bc = BitConverter.GetBytes( (Int32)d.Length );
				ms.Write( bc, 0, bc.Length );
			}
			for( int i = 0; i < encodedData.Count; i++ )
			{
				var d = encodedData[ i ];
				ms.Write( d, 0, d.Length );
			}

			return ms.ToArray();
		}

		private short[] DecodeAudio( byte[] encodedData )
		{
			var decoder = new NSpeex.SpeexDecoder( NSpeex.BandMode.Wide, false );
			var outBuffer = new List<byte[]>();
			var tmpBuffer = new short[ 1024 ];

			int numPackets = BitConverter.ToInt32( encodedData, 0 );

			List<int> packet_sizes = new List<int>();

			for( int i = 0; i < numPackets; i++ )
			{
				int frame_size = BitConverter.ToInt32( encodedData, 4 + ( i * 4 ) );
				packet_sizes.Add( frame_size );
			}

			int fr_index = 0;
			for( var idx = 4 + ( numPackets * 4 ); idx + packet_sizes[ fr_index ] < encodedData.Length; idx += packet_sizes[ fr_index ] )
			{
				var read = decoder.Decode( encodedData, idx, packet_sizes[ fr_index ], tmpBuffer, 0, false );
				var tmpData = new byte[ read * 2 ];
				for( var i = 0; i < read; i++ )
				{
					var ba = BitConverter.GetBytes( tmpBuffer[ i ] );
					System.Array.Copy( ba, 0, tmpData, i * 2, 2 );
				}
				outBuffer.Add( tmpData );
				fr_index++;
			}
			var fullSize = outBuffer.Sum( delegate( byte[] m ) { return m.Length; } );
			var retData = new byte[ fullSize ];
			var offset = 0;
			for( int i = 0; i < outBuffer.Count; i++ )
			{
				var b = outBuffer[ i ];
				System.Array.Copy( b, 0, retData, offset, b.Length );
				offset += b.Length;
			}

			short[] pcm = new short[ retData.Length / sizeof( short ) ];
			Buffer.BlockCopy( retData, 0, pcm, 0, retData.Length );

			return pcm;
		}

		#endregion

		#region ICodec Members

		public byte[] Encode( short[] data, BandMode mode )
		{
			//byte[] d = new byte[ data.Length * 2 ];
			//Buffer.BlockCopy( data, 0, d, 0, d.Length );
			//return encode( d, m_enc );
			//return EncodeAudio( data );
			return encode( data, mode );
		}

		public short[] Decode( byte[] data, BandMode mode )
		{
			//byte[] d = decode( data, m_dec );
			//short[] ret = new short[ d.Length / 2 ];
			//Buffer.BlockCopy( d, 0, ret, 0, d.Length );
			//return ret;
			//return DecodeAudio( data );
			return decode( data, mode );
		}

		#endregion
	}
}